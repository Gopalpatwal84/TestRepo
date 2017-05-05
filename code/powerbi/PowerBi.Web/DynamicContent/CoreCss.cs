using System.Web.Optimization;
using Acom.Mvc.Bundles;
using Acom.Mvc.Cdn;

namespace PowerBI.Web.DynamicContent
{
    public class CoreCss : IBundle
    {
        private readonly CdnConfiguration cdnConfiguration;

        public static BundlePath Path = new BundlePath
        {
            LocalPath = "~/bundles/localcore.css",
            CdnPath = "~/bundles/core.css",
        };

        public CoreCss(CdnConfiguration cdnConfiguration)
        {
            this.cdnConfiguration = cdnConfiguration;
        }

        public void Register(BundleCollection bundleCollection)
        {
            var items = new[]
            {
                "~/css/powerbi/helpers/font-face.css",

                "~/less/sundog-grid/sundog-grid.css",
                "~/less/sundog/sundog.css",
                "~/less/sundog-documentation/sundog-documentation.css",
                "~/less/sundog-navigation/sundog-navigation.css",
                "~/less/sundog/lib/owl.carousel.2.0.0.css",

                "~/css/powerbi/controls/*.css",
                "~/css/powerbi/framework/*.css",

                "~/css/powerbi/blog/post-bio.css",
                "~/css/powerbi/blog/post-date.css",
                "~/css/powerbi/blog/post.css",
                "~/css/powerbi/border.css",
                "~/css/powerbi/card.css",
                "~/css/powerbi/carousel.css",
                "~/css/powerbi/cubes.css",
                "~/css/powerbi/exit-intent.css",
                "~/css/powerbi/footer.css",
                "~/css/powerbi/header.css",
                "~/css/powerbi/heading.css",
                "~/css/powerbi/icon.css",
                "~/css/powerbi/integration.css",
                "~/css/powerbi/integration-logo.css",
                "~/css/powerbi/newsletter.css",
                "~/css/powerbi/olark.css",
                "~/css/powerbi/partners.css",
                "~/css/powerbi/pages/features.css",
                "~/css/powerbi/pages/guided-learning.css",
                "~/css/powerbi/positioning.css",
                "~/css/powerbi/search.css",
                "~/css/powerbi/section.css",
                "~/css/powerbi/social.css",
                "~/css/powerbi/subnav-collapse.css",
                "~/css/powerbi/tag.css",
                "~/css/powerbi/temporary.css",
                "~/css/powerbi/tile.css",
                "~/css/powerbi/video.css",
            };

            var cdnBundle = new StyleBundle(Path.CdnPath).Include(items, new CssRewriteRelativeUrlTransform(Path.CdnPath, hashPrefixedBundle: true));
            cdnBundle.CdnPath = cdnBundle.GetCdnPath(this.cdnConfiguration.GetCurrentCdnPrefix());

            var localBundle = new StyleBundle(Path.LocalPath).Include(items, new CssRewriteRelativeUrlTransform(Path.LocalPath));

            bundleCollection.Add(cdnBundle);
            bundleCollection.Add(localBundle);
        }
    }
}
