namespace BusinessPlatform.Web.Controllers
{
    using System.Web.Mvc;
    using BusinessPlatform.Web.Models.Pages;

    public class ErrorController : BaseController
    {
        public ActionResult Error()
        {
            return this.View("~/Views/_ErrorPage.cshtml", new ViewMetadataModel());
        }
    }
}