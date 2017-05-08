namespace BusinessPlatform.Jobs.Web
{
    using System.Configuration;
    using System.Web.Mvc;
    using WebApi = System.Web.Http;

    public class GlobalAuthFilterConfig
    {
        public static void RegisterMvcGlobalAuthFilter(GlobalFilterCollection filters)
        {
            if (LockdownEnabled())
            {
                filters.Add(new AuthorizeAttribute());
            }
        }

        public static void RegisterWebApiGlobalAuthFilter(WebApi.HttpConfiguration config)
        {
            if (LockdownEnabled())
            {
                config.Filters.Add(new WebApi.AuthorizeAttribute());
            }
        }

        public static bool LockdownEnabled()
        {
            var lockdownEnabled = ConfigurationManager.AppSettings["LockdownEnabled"].Equals("true", System.StringComparison.InvariantCultureIgnoreCase);
            var stagingLockdownEnabled = ConfigurationManager.AppSettings["StagingLockdownEnabled"].Equals("true", System.StringComparison.InvariantCultureIgnoreCase);

            return lockdownEnabled || (Acom.Configuration.KnownSlots.IsStaging && stagingLockdownEnabled);
        }

        public static bool AuthEnabled()
        {
            return ConfigurationManager.AppSettings["AuthEnabled"].Equals("true", System.StringComparison.InvariantCultureIgnoreCase);
        }
    }
}