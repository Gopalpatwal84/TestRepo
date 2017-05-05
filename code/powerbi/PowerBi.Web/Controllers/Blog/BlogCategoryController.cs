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
using Categories = PowerBI.Resources.Pages.Blog.Categories;

namespace PowerBI.Web.Controllers.Blog
{
    [DonutOutputCache(CacheProfile = "BlogCache")]
    public class BlogCategoryController : BlogBaseController
    {
        public BlogCategoryController(
            IRepository<BlogPost> blogPostsRepository,
            IRepository<BlogCategory> blogCategoriesRepository,
            ISearchClient searchClient)
            : base(blogPostsRepository, blogCategoriesRepository, searchClient)
        {
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/blog/category/{slug}/")]
        public async Task<ActionResult> Category(string slug, int page = 1)
        {
            var categoriesTask = this.GetCategories();

            var searchResults =
                await this.ExecuteSearch(
                    () => this.BlogSearchWithFilterByTermInCollection(page, x => x.Categories, slug));

            var categories = await categoriesTask;

            if (searchResults.Results.Any() || searchResults.HasErrors)
            {
                // Default value
                var topicName = string.Empty;

                if (categories.Any())
                {
                    topicName = categories.First(c => string.Equals(c.Slug, slug)).Name;
                }

                var viewModel = new FilteredPosts
                {
                    Header = topicName,
                    BlogPostsList = new PostsList
                    {
                        Title = string.Format(Categories.PostsCategorized, topicName),
                        SmallTitle = false,
                        Posts = searchResults.Results,
                        Categories = await this.GetCategories(),
                        HasErrors = searchResults.HasErrors,
                        Pagination = new Pagination(page, PageSize, searchResults.TotalCount),
                    },
                    NextPrevLinks = new NextPrevLinks(page, PageSize, searchResults.TotalCount)
                };

                return this.View("blog/_results", viewModel);
            }

            return this.NotFound();
        }
    }
}