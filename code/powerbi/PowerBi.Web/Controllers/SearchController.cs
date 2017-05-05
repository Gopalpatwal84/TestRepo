using System.Threading.Tasks;
using System.Web.Mvc;
using Acom.Search.Client.Documents;
using PowerBI.Search.Documents;
using PowerBI.Web.Infrastructure.Helpers;

namespace PowerBI.Web.Controllers
{
    public class SearchController : Controller
    {
        const int PageSize = 25;

        private readonly SearchHelper searchHelper;

        public SearchController(SearchHelper searchHelper)
        {
            this.searchHelper = searchHelper;
        }

        [Route("{culture=en-us}/search/")]
        public async Task<ActionResult> Index(string q, int page = 1)
        {
            var model = await this.searchHelper.Search<Entry>(q, page, PageSize);
            return this.View("search/index", model);
        }

        [Route("{culture=en-us}/search/documentation/")]
        public async Task<ActionResult> Documentation(string q, int page = 1)
        {
            var model = await this.searchHelper.Search<ArticleEntry>(q, page, PageSize);
            return this.View("search/index", model);
        }

        [Route("{culture=en-us}/search/blog/")]
        public async Task<ActionResult> Blog(string q, int page = 1)
        {
            var model = await this.searchHelper.Search<BlogEntry>(q, page, PageSize);
            return this.View("search/index", model);
        }

        [Route("{culture=en-us}/search/community/")]
        public async Task<ActionResult> Community(string q, int page = 1)
        {
            var model = await this.searchHelper.Search<CommunityEntry>(q, page, PageSize);
            return this.View("search/index", model);
        }

        [Route("{culture=en-us}/search/partners/")]
        public async Task<ActionResult> Partners(string q, int page = 1)
        {
            var model = await this.searchHelper.Search<PartnerDirectoryProfileEntry>(q, page, PageSize);
            return this.View("search/index", model);
        }
    }
}