namespace BusinessPlatform.Web.DynamicContent
{
    using System.Web.Optimization;
    using Acom.Mvc.Bundles;
    using BusinessPlatform.Web.Configuration;

    public class CoreScripts : IBundle
    {
        private readonly BusinessPlatformConfiguration businessPlatformConfiguration;

        public CoreScripts(BusinessPlatformConfiguration businessPlatformConfiguration)
        {
            this.businessPlatformConfiguration = businessPlatformConfiguration;
        }

        public static BundlePath Path
        {
            get
            {
                return new BundlePath
                {
                    LocalPath = "~/bundles/localcore.js",
                    CdnPath = "~/bundles/core.js",
                };
            }
        }

        public void Register(BundleCollection bundleCollection)
        {
            var items = new string[]
            {
                "~/scripts/sundog/helpers/*.js",
                "~/scripts/sundog/controls/equalize.js",

                "~/scripts/utilities/*.js",
                "~/scripts/analytics/config/*.js",
                "~/scripts/analytics/libs/*.js",
                "~/scripts/analytics/event-data/*.js",
                "~/scripts/analytics/helpers/*.js",
                "~/scripts/analytics/controls/*.js",
            };

            var cdnBundle = new ScriptBundle(Path.CdnPath).Include(items);
            cdnBundle.CdnPath = cdnBundle.GetCdnPath(this.businessPlatformConfiguration.GetCurrentCdnPrefix());

            var localBundle = new ScriptBundle(Path.LocalPath).Include(items);

            bundleCollection.Add(cdnBundle);
            bundleCollection.Add(localBundle);
        }
    }
}