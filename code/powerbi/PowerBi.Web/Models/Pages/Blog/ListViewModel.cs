using PowerBI.Web.Models.Parts;
using PowerBI.Web.Models.Parts.Blog;

namespace PowerBI.Web.Models.Pages.Blog
{
    public class ListViewModel : ViewMetadataModel
    {
        public PostsList BlogPostsList { get; set; }

        public NextPrevLinks NextPrevLinks { get; set; }
    }
}