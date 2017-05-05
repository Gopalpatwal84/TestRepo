using System;
using System.Linq;
using System.Web;

namespace PowerBI.Web.Infrastructure.Helpers
{
    public static class MobileClientCookieHelper
    {
        private static string _cookieName = "mobileClient";
        private static TimeSpan _cookieValidDuration = TimeSpan.FromHours(1);

        public static void SetMobileClient(this HttpContextBase context, string value)
        {
            context.SetCookie(_cookieName, value, DateTime.UtcNow.Add(_cookieValidDuration));
        }

        public static string GetMobileClient(this HttpContextBase context)
        {
            return context.GetCookie(_cookieName);
        }

        public static string GetMobileClient(this HttpContext context)
        {
            var contextBase = new HttpContextWrapper(context);
            return contextBase.GetMobileClient();
        }

        public static class KnownMobileClient
        {
            private static readonly string _iOS = "iOS".ToLowerInvariant();
            private static readonly string[] _restrictedMobileClient = { _iOS };
            private static readonly string[] _knownMobileClient = { _iOS };

            public static bool IsRestrictedMobileClient(string mobileClient)
            {
                return mobileClient != null && _restrictedMobileClient.Contains(mobileClient.ToLowerInvariant());
            }

            public static bool IsKnownMobileClient(string mobileClient)
            {
                return mobileClient != null && _knownMobileClient.Contains(mobileClient.ToLowerInvariant());
            }
        }
    }
}