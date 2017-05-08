namespace BusinessPlatform.Web.Controllers
{
    using System.Web.Mvc;
    using BusinessPlatform.Web.Models.Pages;
    using DevTrends.MvcDonutCaching;

    [DonutOutputCache(CacheProfile = "SiteCache")]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return this.View("index", new ViewMetadataModel());
        }
    }
}