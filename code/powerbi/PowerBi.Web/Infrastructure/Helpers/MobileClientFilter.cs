using System.Web.Mvc;
using PowerBI.Web.Models.Pages;

namespace PowerBI.Web.Infrastructure.Helpers
{
    public class MobileClientFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var mobileClient = filterContext.HttpContext.Request.QueryString["mobileClient"];

            if (MobileClientCookieHelper.KnownMobileClient.IsKnownMobileClient(mobileClient))
            {
                filterContext.HttpContext.SetMobileClient(mobileClient);
            }
            
            base.OnActionExecuting(filterContext);
        }
    }
}