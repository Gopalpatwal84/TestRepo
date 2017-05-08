namespace BusinessPlatform.Web.Controllers
{
    using System.Web.Mvc;
    using BusinessPlatform.Web.Models.Pages;
    using DevTrends.MvcDonutCaching;

    [DonutOutputCache(CacheProfile = "SiteCache")]
    public class SiteController : BaseController
    {
        public ActionResult Page()
        {
            var viewPath = this.RouteData.Values["viewPath"] as string;

            if (viewPath == null)
            {
                return this.NotFound();
            }

            viewPath = viewPath.TrimEnd('/');

            if (ViewEngines.Engines.FindView(this.ControllerContext, viewPath, null).View == null)
            {
                return this.NotFound();
            }

            return this.View(viewPath, new ViewMetadataModel());
        }
    }
}