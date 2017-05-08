namespace BusinessPlatform.Web.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using BusinessPlatform.Web.Models.Pages;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.OpenIdConnect;

    public class AuthController : Controller
    {
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("auth/signout")]
        public void SignOut()
        {
            this.HttpContext.GetOwinContext().Authentication.SignOut(
                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("auth/unauthorized")]
        public ActionResult Unauthorized()
        {
            return this.View("~/Views/_global/_unauthorized.cshtml", new ViewMetadataModel());
        }
    }
}