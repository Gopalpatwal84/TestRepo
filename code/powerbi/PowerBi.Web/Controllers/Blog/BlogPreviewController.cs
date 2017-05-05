using System.Threading.Tasks;
using System.Web.Mvc;
using Acom.IO;
using Acom.Search.Client;
using DevTrends.MvcDonutCaching;
using Onyx.Client;
using Onyx.Client.Requests.Blog.PowerBI;
using PowerBI.Entities.Blogs;
using PowerBI.Web.Models.Pages.Blog;

namespace PowerBI.Web.Controllers.Blog
{
    [DonutOutputCache(CacheProfile = "BlogCache")]
    public class BlogPreviewController : BlogBaseController
    {
        private readonly IOnyxRequestClient _onyxRequestClient;

        public BlogPreviewController(
            IOnyxRequestClient onyxRequestClient,
            IRepository<BlogPost> blogPostsRepository,
            IRepository<BlogCategory> blogTopicsRepository,
            ISearchClient searchClient)
            : base(blogPostsRepository, blogTopicsRepository, searchClient)
        {
            _onyxRequestClient = onyxRequestClient;
        }

        // TODO: Add the Authorize attribute once the AD authentication is in place.
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [DonutOutputCache(NoStore = true, Duration = 0)]
        [Route("{culture=en-us}/blog/{slug}/preview/")]
        [Route("{culture=en-us}/blog/{blogName}/{slug}/preview/")]
        [Route("{culture=en-us}/blog/preview/{slug}/", Name = "DefaultBlogPreviewRoute")]
        [Route("{culture=en-us}/blog/preview/{blogName}/{slug}/")]
        public async Task<ActionResult> Preview(string slug, string blogName = null)
        {
            if (string.Equals(blogName, "blog"))
            {
                // If the slug received in the preview endpoint contains the blog name we need to remove it from the URL
                // To do this we answer with a redirect to this same endpoint without the blog name
                return this.RedirectToRoute("DefaultBlogPreviewRoute", new { slug });
            }

            var post = await _onyxRequestClient.GetPowerBiBlogPost(slug);

            if (post == null)
            {
                // The slug of the post might contain the blog name so we try again adding it
                post = await _onyxRequestClient.GetPowerBiBlogPost($"blog/{slug}");
 
                if (post == null)
                {
                    return this.NotFound();
                }
            }

            var viewModel = new Details
            {
                Post = post.ToEntity(),
                Content = post.Content,
                AllCategories = await GetCategories(),
                IsInPreview = true,
            };

            return this.View("blog/_details", viewModel);
        }
    }
}