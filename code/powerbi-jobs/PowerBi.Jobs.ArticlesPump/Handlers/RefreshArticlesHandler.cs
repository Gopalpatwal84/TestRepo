using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Acom.Articles;
using Acom.IO.Collections;
using Acom.IO.Json;
using Acom.Redis;
using Acom.Search.Queue.Indexing;
using Mediation;
using Mediation.Handlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PowerBI.Entities;
using PowerBI.Messages.Refresh;
using PowerBI.Search.Documents;
using Serilog;

namespace PowerBI.Jobs.ArticlesPump.Handlers
{
    public class RefreshArticlesHandler : IHandleCommand<RefreshArticlesCommand>
    {
        private readonly IRedisClient redisClient;
        private readonly ILogger logger;
        private readonly ArticlesReader<Article> articlesReader;
        private readonly IMediator mediator;
        private readonly JsonLoader<Course> coursesJsonLoader;

        public RefreshArticlesHandler(
            IRedisClient redisClient,
            ILogger logger,
            ArticlesReader<Article> articlesReader,
            IMediator mediator,
            JsonLoader<Course> coursesJsonLoader)
        {
            this.redisClient = redisClient;
            this.logger = logger;
            this.articlesReader = articlesReader;
            this.mediator = mediator;
            this.coursesJsonLoader = coursesJsonLoader;
        }

        public async Task Handle(RefreshArticlesCommand command)
        {
            this.logger.Verbose("Retrieving articles from Blob container.");

            var articles = await this.articlesReader.RequestArticles();

            if (!articles.Any())
            {
                this.logger.Warning("No articles found, cancelling task");
                return;
            }

            var guidedLearningSlugs = this.coursesJsonLoader.Data.Value.SelectMany(x => x.ArticleSlugs);

            articles.ToList().ForEach(article =>
            {
                article.Authors = this.ParseArticleAuthors(article.ArticleAuthor);
                article.Contributors = this.ParseGitHubContributors(article.GitHubContributors).ToArray();
                article.LastModified = this.ParseLastUpdated(article.Tags);

                var articleUrlPattern = guidedLearningSlugs.Contains(article.Slug) ? "/guided-learning/{0}/" : "/documentation/{0}/";
                article.Url = string.Format(articleUrlPattern, article.Slug);
            });

            this.logger.Information("{ArticlesCount} articles fetched", articles.Length);
            await this.redisClient.SetAllAsync(articles.OrderByDescending(x => x.LastModified));
            
            this.logger.Verbose("Queuing messages to update the Azure Search index...");
            await articles
                .OrderByDescending(x => x.LastModified)
                .ForEachAsync(async article =>
                {
                    var articleContents = await this.articlesReader.RequestArticleContent(article);
                    var entry = CreateEntry(article, articleContents);

                    await Task.WhenAll(
                        this.mediator.Send(new IndexItemCommand(entry, IndexAction.UpdateOrInsert)),
                        this.redisClient.UpdateSetAsync(articleContents, (x, i) => i));
                });

            this.logger.Verbose("Job done!");
        }

        private DateTime? ParseLastUpdated(Dictionary<string, string> tags)
        {
            var dateString = string.Empty;

            if (tags?.TryGetValue("ms.date", out dateString) == true)
            {
                DateTime lastUpdatedDate;
                if (DateTime.TryParse(dateString, out lastUpdatedDate))
                {
                    return lastUpdatedDate;
                }
            }

            return null;
        }

        private static ArticleEntry CreateEntry(Article article, ArticleContent[] articleContents)
        {
            var entry = new ArticleEntry(article.Url, article.Slug)
            {
                Title = article.Title,
                Content = articleContents.First(x => x.Culture == "en-us").Content,
                Created = article.Created,
                Modified = article.LastModified ?? article.Created,
                Summary = article.Description,
                ExtraData = new ArticleEntry.ArticleEntryExtraData(),
            };

            if (article.Duration.HasValue)
            {
                entry.ExtraData = new ArticleEntry.ArticleEntryExtraData { Duration = article.Duration };
                entry.InternalTags = new[] { PowerBiEntry.GuidedLearningTag };
            }

            foreach (var culture in article.AlternateCultures.Keys)
            {
                entry.TitleLanguages[culture] = article.AlternateCultures[culture].Title;
                entry.SummaryLanguages[culture] = article.AlternateCultures[culture].Description;
            }

            foreach (var articleContent in articleContents.Where(x => x.Culture != "en-us"))
            {
                entry.ContentLanguages[articleContent.Culture] = articleContent.Content;
            }

            return entry;
        }

        private string[] ParseArticleAuthors(string articleAuthorString)
        {
            if (string.IsNullOrWhiteSpace(articleAuthorString))
            {
                return new string[0];
            }

            return articleAuthorString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()).ToArray();
        }

        private IEnumerable<GithubAuthor> ParseGitHubContributors(string githubContributorsString)
        {
            if (string.IsNullOrEmpty(githubContributorsString))
            {
                return Enumerable.Empty<GithubAuthor>();
            }

            try
            {
                var obj = JToken.Parse(githubContributorsString);
                if (obj is JArray)
                {
                    var contributors = obj.ToObject<List<GithubAuthor>>();
                    contributors.ForEach(c => c.Name = WebUtility.HtmlDecode(c.Name));
                    return contributors;
                }

                // Article hasn't been updated with the new metadata structure Key: Login, Value: Id
                var dictionary = obj.ToObject<Dictionary<string, string>>();
                return dictionary.Select(x => new GithubAuthor() { Name = x.Key, Login = x.Key, Id = x.Value });
            }
            catch (JsonException exception)
            {
                logger.Error(exception, "ARTICLES PARSE GITHUB CONTRIBUTORS: {GithubContributors}", githubContributorsString);
                return Enumerable.Empty<GithubAuthor>();
            }
        }
    }
}
