using System.Collections.Generic;
using PowerBI.Web.Models.Pages.Blog;

namespace PowerBI.Web.Models.Parts.Blog
{
    public class BlogSearchResult
    {
        public bool HasErrors { get; set; }

        public IEnumerable<BlogPostModel> Results { get; set; }

        public int TotalCount { get; set; }

        public static readonly BlogSearchResult ErrorResult = new BlogSearchResult
        {
            TotalCount = 0,
            Results = new List<BlogPostModel>(),
            HasErrors = true
        };
    }
}