using System.Web.Mvc;
using PowerBI.Web.Models.Pages;

namespace PowerBI.Web.Controllers
{
    public class AuthController : Controller
    {
        [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("auth/unauthorized")]
        public ActionResult Unauthorized()
        {
            return this.View("~/Views/_global/_unauthorized.cshtml", new ViewMetadataModel());
        }
    }
}