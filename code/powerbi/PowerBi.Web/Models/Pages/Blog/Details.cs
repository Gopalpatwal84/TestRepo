using System.Collections.Generic;
using PowerBI.Entities.Blogs;
using PowerBI.Web.Models.Parts;

namespace PowerBI.Web.Models.Pages.Blog
{
    public class Details : ViewMetadataModel
    {
        public BlogPost Post { get; set; }

        public string Content { get; set; }

        public IEnumerable<BlogCategory> AllCategories { get; set; }

        public Disqus DisqusModel { get; set; }

        public bool IsInPreview { get; set; }

        public Social SocialModel { get; set; }
    }
}