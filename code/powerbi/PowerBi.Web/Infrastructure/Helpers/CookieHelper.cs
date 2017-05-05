using System;
using System.Web;

namespace PowerBI.Web.Infrastructure.Helpers
{
    public static class CookieHelper
    {
        public static void SetCookie(this HttpContextBase context, string cookieName, string value, DateTime cookieExpiration)
        {
            var cookie = new HttpCookie(cookieName, value.ToLowerInvariant());
            cookie.Expires = cookieExpiration;
            context.Response.Cookies.Set(cookie);
        }

        public static string GetCookie(this HttpContextBase context, string cookieName)
        {
            return context.Request.Cookies[cookieName]?.Value ?? string.Empty;
        }
    }
}