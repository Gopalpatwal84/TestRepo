using System.Web.Mvc;
using Acom.Mvc.Filters;
using PowerBI.Web.Models.Pages;

namespace PowerBI.Web.Controllers
{
    [CultureRouteFilter]
    [ModelHydrator]
    public class BaseController : Controller
    {
        public ActionResult NotFound()
        {
            return this.View("_404", new ViewMetadataModel());
        }
    }
}