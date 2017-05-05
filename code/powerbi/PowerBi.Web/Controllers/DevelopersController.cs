using Acom.IO;
using PowerBI.Entities.Blogs;
using PowerBI.Web.Models.Pages;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DevTrends.MvcDonutCaching;

namespace PowerBI.Web.Controllers
{
    [DonutOutputCache(CacheProfile = "SiteCache")]
    public class DevelopersController : BaseController
    {
        private readonly IRepository<BlogPost> blogPostsRepository;

        public DevelopersController(IRepository<BlogPost> blogPostsRepository)
        {
            this.blogPostsRepository = blogPostsRepository;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/developers/")]
        public async Task<ActionResult> Index()
        {
            var posts = await this.blogPostsRepository.GetAsync();
            var developerPosts = posts?.Where(p => p.Categories.Any(c => c.Slug.Equals("developers", StringComparison.InvariantCultureIgnoreCase))).Take(5);
            var viewModel = new DevelopersViewModel
            {
                BlogPosts = developerPosts,
            };

            return this.View("developers", viewModel);
        }
    }
}