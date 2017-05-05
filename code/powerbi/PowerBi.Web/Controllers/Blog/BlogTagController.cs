using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Acom.IO;
using Acom.IO.Json;
using Acom.Search.Client;
using DevTrends.MvcDonutCaching;
using PowerBI.Entities.Blogs;
using PowerBI.Web.Models;
using PowerBI.Web.Models.Pages.Blog;
using PowerBI.Web.Models.Parts;
using PowerBI.Web.Models.Parts.Blog;
using TagResources = PowerBI.Resources.Pages.Blog.Tag;

namespace PowerBI.Web.Controllers.Blog
{
    [DonutOutputCache(CacheProfile = "BlogCache")]
    public class BlogTagController : BlogBaseController
    {
        public BlogTagController(
            IRepository<BlogPost> blogPostsRepository,
            IRepository<BlogCategory> blogCategoriesRepository,
            ISearchClient searchClient)
            : base(blogPostsRepository, blogCategoriesRepository, searchClient)
        {
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/blog/tag/{slug}/")]
        public async Task<ActionResult> Tag(string slug, int page = 1)
        {
            var searchResults =
                await this.ExecuteSearch(() => this.BlogSearchWithFilterByTermInCollection(page, x => x.Tags, slug));

            if (searchResults.Results.Any() || searchResults.HasErrors)
            {
                var blogPostEntry = searchResults.Results.First();
                var blogPost = await BlogPostsRepository.GetAsync(blogPostEntry.Slug);
                var tagName = blogPost.Tags.ResolveFromSlug(slug).Name;

                var viewModel = new FilteredPosts
                {
                    Header = tagName,
                    BlogPostsList = new PostsList
                    {
                        Title = string.Format(TagResources.PostsTagged, tagName),
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