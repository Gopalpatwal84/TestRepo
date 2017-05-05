namespace PowerBI.Web.Features.UserInfoCookie
{
    using System.Web.Mvc;

    public class UserInfoCookieFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Request.UpdateUserInfoCookie();
        }
    }
}