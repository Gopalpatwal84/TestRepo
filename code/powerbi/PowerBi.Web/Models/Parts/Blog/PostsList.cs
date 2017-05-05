using System.Collections.Generic;
using PowerBI.Entities.Blogs;
using PowerBI.Web.Models.Pages.Blog;

namespace PowerBI.Web.Models.Parts.Blog
{
    public class PostsList
    {
        public string Title { get; set; }

        public bool SmallTitle { get; set; }

        public IEnumerable<BlogPostModel> Posts { get; set; }

        public IEnumerable<BlogCategory> Categories { get; set; }

        public Pagination Pagination { get; set; }

        public bool HasErrors { get; set; }
    }
}