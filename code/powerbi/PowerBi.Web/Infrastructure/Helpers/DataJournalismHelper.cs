using System;
using System.Web;

namespace PowerBI.Web.Infrastructure.Helpers
{
    public static class DataJournalismHelper
    {
        private static string _cookieName = "datajournalismRegistration";
        private static string _completedValue = "complete";

        public static bool IsDataJournalismRegistrationComplete(this HttpContextBase context)
        {
            if (context.Request.Cookies[_cookieName] != null)
            {
                return _completedValue.Equals(context.Request.Cookies.Get(_cookieName).Value, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public static bool IsDataJournalismRegistrationComplete(this HttpContext context)
        {
            var contextBase = new HttpContextWrapper(context);
            return contextBase.IsDataJournalismRegistrationComplete();
        }

        public static void SetDataJournalismRegistrationComplete(this HttpContextBase context)
        {
            var cookie = new HttpCookie(_cookieName, _completedValue);
            cookie.Expires = DateTime.UtcNow.AddYears(2);
            context.Response.Cookies.Set(cookie);
        }

        public static void SetDataJournalismRegistrationComplete(this HttpContext context)
        {
            var contextBase = new HttpContextWrapper(context);
            contextBase.SetDataJournalismRegistrationComplete();
        }
    }
}