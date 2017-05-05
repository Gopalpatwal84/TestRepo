using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Acom.IO;
using Acom.Search.Client;
using DevTrends.MvcDonutCaching;
using PowerBI.Entities.Blogs;
using PowerBI.Web.Models;
using PowerBI.Web.Models.Pages.Blog;
using PowerBI.Web.Models.Parts;
using PowerBI.Web.Models.Parts.Blog;
using SharedResources = PowerBI.Resources.Shared.Blog;

namespace PowerBI.Web.Controllers.Blog
{
    [DonutOutputCache(CacheProfile = "BlogCache")]
    public class BlogIndexController : BlogBaseController
    {
        private readonly IRepository<FeaturedBlogPost> _featuredBlogPostRepository;

        public BlogIndexController(
            IRepository<FeaturedBlogPost> featuredBlogPostRepository,
            IRepository<BlogPost> blogPostsRepository,
            IRepository<BlogCategory> blogCategoriesRepository,
            ISearchClient searchClient)
            : base(blogPostsRepository, blogCategoriesRepository, searchClient)
        {
            _featuredBlogPostRepository = featuredBlogPostRepository;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/blog/")]
        public async Task<ActionResult> Index(int page = 1)
        {
            var searchResults = await this.FromOrderedSet(page);

            var featuredPost = await _featuredBlogPostRepository.GetAsync(FeaturedBlogPost.DefaultFeaturedSlug);

            // Removes the featured blog posts from the results
            searchResults.Results = searchResults.Results.Where(x => featuredPost == null || x.Slug != featuredPost.Post.Slug);

            var viewModel = new Index
            {
                BlogPostsList = new PostsList
                {
                    Title = SharedResources.RecentPosts,
                    SmallTitle = true,
                    Posts = searchResults.Results,
                    Categories = await this.GetCategories(),
                    HasErrors = searchResults.HasErrors,
                    Pagination = new Pagination(page, PageSize, searchResults.TotalCount),
                },
                FeaturedPost = featuredPost,
                NextPrevLinks = new NextPrevLinks(page, PageSize, searchResults.TotalCount)
            };

            return this.View("blog/index", viewModel);
        }
    }
}