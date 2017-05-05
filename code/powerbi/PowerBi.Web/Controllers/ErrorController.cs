using System.Web.Mvc;
using PowerBI.Web.Models.Pages;

namespace PowerBI.Web.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Error()
        {
            return this.View("_ErrorPage", new ViewMetadataModel());
        }
    }
}