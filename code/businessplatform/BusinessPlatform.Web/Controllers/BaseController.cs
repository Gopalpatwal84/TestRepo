namespace BusinessPlatform.Web.Controllers
{
    using System.Web.Mvc;
    using Acom.Mvc.Filters;
    using BusinessPlatform.Web.Models.Pages;

    [CultureRouteFilter]
    public class BaseController : Controller
    {
        public ActionResult NotFound()
        {
            return this.View("_404", new ViewMetadataModel());
        }
    }
}