using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Acom.IO;
using DevTrends.MvcDonutCaching;
using PowerBI.Entities;
using PowerBI.Web.Models.Pages.Industries;

namespace PowerBI.Web.Controllers
{
    [DonutOutputCache(CacheProfile = "SiteCache")]
    public class IndustriesController : BaseController
   {
        private readonly IRepository<Industry> industriesRepository;

        public IndustriesController(IRepository<Industry> industriesRepository)
        {
            this.industriesRepository = industriesRepository;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/industries/")]
        public async Task<ActionResult> Index()
        {
            var industries = await this.industriesRepository.GetAsync();

            var model = new IndustriesViewModel
            {
                Industries = industries.ToArray(),
            };

            return this.View("industries/index", model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/industries/{slug}/")]
        public async Task<ActionResult> Details(string slug)
        {
            var industry = await this.industriesRepository.GetAsync(slug);
            if (industry == null)
            {
                return this.NotFound();
            }

            var model = new IndustryDetailsViewModel
            {
                Industry = industry
            };

            return this.View("industries/_details", model);
        }
    }
}