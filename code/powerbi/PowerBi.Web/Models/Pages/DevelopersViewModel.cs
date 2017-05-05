using PowerBI.Entities.Blogs;
using System.Collections.Generic;

namespace PowerBI.Web.Models.Pages
{
    public class DevelopersViewModel : ViewMetadataModel
    {
        public IEnumerable<BlogPost> BlogPosts { get; set; }
    }
}