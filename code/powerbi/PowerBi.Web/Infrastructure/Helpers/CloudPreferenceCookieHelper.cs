using System;
using System.Collections.Generic;
using System.Web;

namespace PowerBI.Web.Infrastructure.Helpers
{
    public static class CloudPreferenceCookieHelper
    {
        private static string _cookieName = "cloudPreference";

        public static bool IsCloudPreference(this HttpContextBase context, string value)
        {
            return string.Equals(context.GetCloudPreference(), value, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsCloudPreference(this HttpContext context, string value)
        {
            var contextBase = new HttpContextWrapper(context);
            return contextBase.IsCloudPreference(value);
        }

        public static void SetCloudPreference(this HttpContextBase context, string value)
        {
            context.SetCookie(_cookieName, value, DateTime.UtcNow.AddYears(2));
        }

        public static void SetCloudPreference(this HttpContext context, string value)
        {
            var contextBase = new HttpContextWrapper(context);
            contextBase.SetCloudPreference(value);
        }

        public static string GetCloudPreference(this HttpContextBase context)
        {
            return context.GetCookie(_cookieName);
        }

        public static string GetCloudPreference(this HttpContext context)
        {
            var contextBase = new HttpContextWrapper(context);
            return contextBase.GetCloudPreference();
        }
    }
    
    public static class KnownClouds
    {
        public static readonly string China = "china";

        public static List<string> ValidClouds = new List<string> { China };
    }
}