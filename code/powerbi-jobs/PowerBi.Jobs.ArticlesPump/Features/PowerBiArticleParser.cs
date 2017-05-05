using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Acom.Articles;
using PowerBI.Entities;

namespace PowerBI.Jobs.ArticlesPump.Features
{
    public class PowerBiArticleParser : ArticleParser<Article>
    {
        protected override Dictionary<string, Expression<Func<Article, object>>> CreateMetadataDictionary()
        {
            return new Dictionary<string, Expression<Func<Article, object>>>
            {
                { "articleTitle", article => article.Title },
                { "articleAuthor", article => article.ArticleAuthor },
                { "courseDuration", article => article.Duration },
                { "createdDate", article => article.Created },
                { "featuredVideoId", article => article.VideoId },
                { "gitHubContributors", article => article.GitHubContributors },
                { "metaDescription", article => article.Description },
                { "tags", article => article.Tags }
            };
        }
    }
}