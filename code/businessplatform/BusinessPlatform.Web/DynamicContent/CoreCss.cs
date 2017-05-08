namespace BusinessPlatform.Web.DynamicContent
{
    using System.Web.Optimization;
    using Acom.Mvc.Bundles;
    using BusinessPlatform.Web.Configuration;

    public class CoreCss : IBundle
    {
        private readonly BusinessPlatformConfiguration businessPlatformConfiguration;

        public CoreCss(BusinessPlatformConfiguration businessPlatformConfiguration)
        {
            this.businessPlatformConfiguration = businessPlatformConfiguration;
        }

        public static BundlePath Path
        {
            get
            {
                return new BundlePath
                {
                    LocalPath = "~/bundles/localcore.css",
                    CdnPath = "~/bundles/core.css",
                };
            }
        }

        public void Register(BundleCollection bundleCollection)
        {
            var items = new string[]
            {
                "~/css/sundog-grid/sundog-grid.css",
                "~/css/sundog/sundog.css",
                "~/css/sundog-navigation/sundog-navigation.css",
                "~/css/businessplatform/controls/navigation-themed.css",
                "~/css/businessplatform/framework/link.css",
                "~/css/businessplatform/framework/section.css",
                "~/css/businessplatform/framework/typography.css",
                "~/css/businessplatform/header.css",
                "~/css/businessplatform/temporary.css",
            };

            var cdnBundle = new StyleBundle(Path.CdnPath).Include(items, new CssRewriteRelativeUrlTransform(Path.CdnPath, hashPrefixedBundle: true));
            cdnBundle.CdnPath = cdnBundle.GetCdnPath(this.businessPlatformConfiguration.GetCurrentCdnPrefix());

            var localBundle = new StyleBundle(Path.LocalPath).Include(items, new CssRewriteRelativeUrlTransform(Path.LocalPath));

            bundleCollection.Add(cdnBundle);
            bundleCollection.Add(localBundle);
        }
    }
}