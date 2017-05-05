using System.Web.Mvc;
using DevTrends.MvcDonutCaching;
using PowerBI.Web.Models.Pages;

namespace PowerBI.Web.Controllers
{
    [DonutOutputCache(CacheProfile = "SiteCache")]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return this.View("index", new ViewMetadataModel());
        }
    }
}