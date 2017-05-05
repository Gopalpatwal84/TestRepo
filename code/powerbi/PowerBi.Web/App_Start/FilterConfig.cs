namespace PowerBI.Web
{
    using System;
    using System.Configuration;
    using System.Web.Mvc;
    using Acom.Mvc.Filters;
    using Infrastructure.Helpers;

    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if (LockdownEnabled())
            {
                filters.Add(new AuthorizeAttribute());
            }

            filters.Add(new AddVersionHeadersFilter());
            filters.Add(new CloudPreferenceFilter());
            filters.Add(new MobileClientFilter());
        }

        public static bool LockdownEnabled()
        {
            var lockdownEnabled = ConfigurationManager.AppSettings["LockdownEnabled"].Equals("true", StringComparison.InvariantCultureIgnoreCase);
            var stagingLockdownEnabled = ConfigurationManager.AppSettings["StagingLockdownEnabled"].Equals("true", StringComparison.InvariantCultureIgnoreCase);

            return lockdownEnabled || (Acom.Configuration.KnownSlots.IsStaging && stagingLockdownEnabled);
        }

        public static bool AuthEnabled()
        {
            return ConfigurationManager.AppSettings["AuthEnabled"].Equals("true", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}