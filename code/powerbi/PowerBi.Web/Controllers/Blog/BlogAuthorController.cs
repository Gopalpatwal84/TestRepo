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
using PageResources = PowerBI.Resources.Pages.Blog.Author;

namespace PowerBI.Web.Controllers.Blog
{
    [DonutOutputCache(CacheProfile = "BlogCache")]
    public class BlogAuthorController : BlogBaseController
    {
        public BlogAuthorController(
            IRepository<BlogPost> blogPostsRepository,
            IRepository<BlogCategory> blogCategoriesRepository,
            ISearchClient searchClient)
            : base(blogPostsRepository, blogCategoriesRepository, searchClient)
        {
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/blog/author/{slug}/")]
        public async Task<ActionResult> Author(string slug, int page = 1)
        {
            var searchResults =
                await this.ExecuteSearch(() => this.BlogSearchWithFilterByProperty(page, x => x.PublisherSlug, slug));

            if (searchResults.Results.Any() || searchResults.HasErrors)
            {
                // Default value
                var authorName = slug;
                var header = string.Format(PageResources.PostsBy, string.Empty);

                if (!searchResults.HasErrors)
                {
                    authorName = searchResults.Results.First().Author.Name;
                    header = string.Format(PageResources.PostsBy, authorName);
                }

                var viewModel = new FilteredPosts
                {
                    Header = authorName,
                    BlogPostsList = new PostsList
                    {
                        Title = header,
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