using System;
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
using PageResources = PowerBI.Resources.Pages.Blog.Archives;

namespace PowerBI.Web.Controllers.Blog
{
    [DonutOutputCache(CacheProfile = "BlogCache")]
    public class BlogArchivesController : BlogBaseController
    {
        public BlogArchivesController(
            IRepository<BlogPost> blogPostsRepository,
            IRepository<BlogContent> blogContentRepository,
            IRepository<BlogCategory> blogCategoriesRepository,
            ISearchClient searchClient)
            : base(blogPostsRepository, blogCategoriesRepository, searchClient)
        {
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/blog/{year:regex(^[0-9]{4}$)}/{month:regex(^[0-9]{1,2}$)}/")]
        public async Task<ActionResult> Archives(int year, int month, int page = 1)
        {
            DateTime startDate, endDate;
            try
            {
                startDate = new DateTime(year, month, 1);
                endDate = startDate.AddMonths(1);
            }
            catch (ArgumentOutOfRangeException)
            {
                // Not a valid year / month pair
                return this.NotFound();
            }

            var searchResults = await this.ExecuteSearch(() => this.BlogSearchInRange(page, startDate, endDate));

            var viewModel = new FilteredPosts
            {
                Header = startDate.ToString("MMMM yyyy"),
                BlogPostsList = new PostsList
                {
                    Title = string.Format(PageResources.MonthlyArchives, startDate.ToString("MMMM"), year),
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
    }
}