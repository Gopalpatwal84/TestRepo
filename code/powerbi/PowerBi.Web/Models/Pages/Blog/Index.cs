using PowerBI.Entities.Blogs;

namespace PowerBI.Web.Models.Pages.Blog
{
    public class Index: ListViewModel
    {
        public FeaturedBlogPost FeaturedPost { get; set; }
    }
}