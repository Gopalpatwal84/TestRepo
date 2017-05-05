using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Acom.Articles;
using Acom.IO;
using DevTrends.MvcDonutCaching;
using PowerBI.Entities;
using PowerBI.Web.Configuration;
using PowerBI.Web.Models;
using PowerBI.Web.Models.Pages.Documentation;

using static Acom.IO.Entities.CultureHelperBase;

namespace PowerBI.Web.Controllers
{
    [DonutOutputCache(CacheProfile = "SiteCache")]
    public class DocumentationController : BaseController
    {
        private readonly IRepository<Lefty> leftyRepository;
        private readonly IRepository<Article> articlesRepository;
        private readonly IRepository<ArticleContent> articleContentsRepository;
        private readonly PowerBIConfiguration powerbiConfiguration;

        public DocumentationController(
            IRepository<Lefty> leftyRepository,
            IRepository<Article> articlesRepository,
            IRepository<ArticleContent> articleContentsRepository, 
            PowerBIConfiguration powerbiConfiguration)
        {
            this.leftyRepository = leftyRepository;
            this.articlesRepository = articlesRepository;
            this.articleContentsRepository = articleContentsRepository;
            this.powerbiConfiguration = powerbiConfiguration;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/documentation/{slug}/")]
        public async Task<ActionResult> Detail(string slug)
        {
            var article = await this.articlesRepository.GetAsync(slug);
            if (article == null)
            {
                return this.NotFound();
            }

            article = article.OverrideArticleCulture(GetCurrentCulture());
            
            var content = await this.articleContentsRepository.GetAsync($"{GetCurrentCulture()}-{slug}") ??
                          await this.articleContentsRepository.GetAsync($"{GetDefaultCulture()}-{slug}");

            var lefties = await this.leftyRepository.GetAsync();
            var lefty = lefties.FirstOrDefault(x => x.Maps.Any(area => area.Value.Articles.Any(link => link.Value == $"article:{slug}")));

            if (lefty == null)
            {
                lefty = await this.leftyRepository.GetAsync("service");
            }

            var viewModel = new Details
            {
                Article = article,
                Contributors = new GithubAuthor[0],
                Content = content?.Content,
                LastModified = article.LastModified ?? article.Created,
                LeftNavigation = new LeftNavigation
                {
                    Lefty = lefty,
                    ActiveSlug = $"article:{slug}",
                },
                FeedbackEnabled = this.powerbiConfiguration.ArticlesFeedbackEnabled,
            };

            foreach (var tag in article.Tags)
            {
                viewModel.AdditionalMetaTags.Add(tag.Key, tag.Value);
            }

            if (article.Authors?.Any() == true && article.Contributors?.Any() == true)
            {
                var authors = article.Authors.SelectMany(author =>
                    article.Contributors.Where(contributor =>
                        string.Equals(contributor?.Login, author, StringComparison.InvariantCultureIgnoreCase)))
                    .Where(x => x != null)
                    .ToArray();

                viewModel.Contributors = authors.Union(article.Contributors.Except(authors)).ToArray();
                viewModel.GitHubAvatarUrl = string.Format(this.powerbiConfiguration.GitHubAvatarUrl, viewModel.Contributors.First().Id, "36");
                viewModel.GitHubProfileUrl = string.Format(this.powerbiConfiguration.GitHubProfileUrl, viewModel.Contributors.First().Login);
            }

            return this.View("documentation/_details", viewModel);
        }
    }
}