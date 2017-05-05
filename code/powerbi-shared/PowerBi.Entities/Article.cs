using System;
using System.Collections.Generic;
using Acom.Articles;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    [Serializable]
    public class Article : IHaveSlug, IHaveDetailPage, IArticle<Article>
    {
        public string ArticleAuthor { get; set; }

        public string[] Authors { get; set; }

        public string GitHubContributors { get; set; }

        public GithubAuthor[] Contributors { get; set; }

        public string Slug { get; set; }

        public string BlobName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string VideoId { get; set; }

        public Dictionary<string, Article> AlternateCultures { get; set; }
        
        public DateTime? LastModified { get; set; }

        public DateTime Created { get; set; }

        public TimeSpan? Duration { get; set; }

        public Dictionary<string, string> Tags { get; set; }

        public string Url { get; set; }
    }
}
