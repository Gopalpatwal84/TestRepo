namespace BusinessPlatform.Web
{
    using System;
    using System.Configuration;
    using System.Web.Mvc;
    using Acom.Mvc.Filters;

    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if (LockdownEnabled())
            {
                filters.Add(new AuthorizeAttribute());
            }

            filters.Add(new AddVersionHeadersFilter());
        }

        public static bool LockdownEnabled()
        {
            var lockdownEnabled = ConfigurationManager.AppSettings["LockdownEnabled"].Equals("true", StringComparison.InvariantCultureIgnoreCase);
            var stagingLockdownEnabled = ConfigurationManager.AppSettings["StagingLockdownEnabled"].Equals("true", StringComparison.InvariantCultureIgnoreCase);

            return lockdownEnabled || (Acom.Configuration.KnownSlots.IsStaging && stagingLockdownEnabled);
        }

        public static bool AuthEnabled()
        {
            return ConfigurationManager.AppSettings["AuthEnabled"].Equals("true", System.StringComparison.InvariantCultureIgnoreCase);
        }
    }
}