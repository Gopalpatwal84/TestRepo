using System;
using PowerBI.Entities.Blogs;

namespace PowerBI.Web.Models.Pages.Blog
{
    public class BlogPostModel
    {
        public bool? IsFeatured { get; set; }

        public string Url { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }

        public BlogAuthor Author { get; set; }

        public DateTimeOffset Published { get; set; }

        public string Summary { get; set; }

        public string[] Tags { get; set; }

        public string[] Categories { get; set; }
    }
}