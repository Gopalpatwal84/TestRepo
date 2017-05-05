using System.Threading.Tasks;
using System.Web.Mvc;
using Acom.IO;
using DevTrends.MvcDonutCaching;
using PowerBI.Entities;
using PowerBI.Web.Models.Pages;

namespace PowerBI.Web.Controllers
{
    [DonutOutputCache(CacheProfile = "CloudPreferenceCache")]
    public class MobileController : BaseController
    {
        private readonly IRepository<AndroidApp> _androidAppsRepository;

        public MobileController(IRepository<AndroidApp> androidAppsRepository)
        {
            _androidAppsRepository = androidAppsRepository;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/mobile/")]
        public async Task<ActionResult> Index()
        {
            var androidApps = await _androidAppsRepository.GetAsync();

            return View("mobile/index", new MobileIndexViewModel {
                AndroidApps = androidApps
            });
        }
    }
}