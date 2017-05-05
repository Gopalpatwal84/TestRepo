using System.Web.Optimization;
using Acom.Mvc.Bundles;
using Acom.Mvc.Cdn;
using PowerBI.Web.Configuration;

namespace PowerBI.Web.DynamicContent
{
    public class CoreScripts : IBundle
    {
        private readonly CdnConfiguration cdnConfiguration;

        public static BundlePath Path = new BundlePath
        {
            LocalPath = "~/bundles/localcore.js",
            CdnPath = "~/bundles/core.js",
        };

        public CoreScripts(CdnConfiguration cdnConfiguration)
        {
            this.cdnConfiguration = cdnConfiguration;
        }

        public void Register(BundleCollection bundleCollection)
        {
            var items = new[]
            {
                "~/scripts/_variables.js",

                "~/scripts/sundog/lib/owl.carousel.2.0.0.min.js",

                "~/scripts/form.js",
                "~/scripts/utilities/*.js",
                "~/scripts/analytics/config/*.js",
                "~/scripts/analytics/libs/*.js",
                "~/scripts/analytics/event-data/*.js",
                "~/scripts/analytics/helpers/*.js",
                "~/scripts/analytics/controls/*.js",

                "~/scripts/sundog/helpers/*.js",
                "~/scripts/sundog-documentation/controls/navigationScroll.js", // has to position scollspy nav before scrollspy runs
                "~/scripts/sundog-documentation/controls/navigationLeft.js",
                "~/scripts/sundog/controls/equalize.js",
                "~/scripts/sundog/controls/flyout.js",
                "~/scripts/sundog/controls/modal.js",
                "~/scripts/sundog/controls/scrollspy.js",
                "~/scripts/sundog/controls/tabs.js",
                "~/scripts/sundog/controls/toggle.js",
                "~/scripts/sundog/framework/deeplink.js",
                "~/scripts/sundog/framework/localStorage.js",
                "~/scripts/sundog/framework/socialShare.js",
                "~/scripts/sundog-navigation/controls/navigation.js",

                "~/scripts/consentForm.js",
                "~/scripts/culturesDropdown.js",
                "~/scripts/currencyDropdown.js",
                "~/scripts/features.js",
                "~/scripts/exit-intent.js",
                "~/scripts/guidedLearning.js",

                "~/scripts/hoverGif.js",
                "~/scripts/integration.js",
                "~/scripts/powerbi.js",
                "~/scripts/powerbi/controls/carousel.js",
                "~/scripts/powerbi/controls/navigationLeft.js",
                "~/scripts/powerbi/feedback.js",
                "~/scripts/price.js",
                "~/scripts/signupLink.js",
                "~/scripts/subnavCollapse.js",
                "~/scripts/supportForm.js",
                "~/scripts/tagDecorator.js",
                "~/scripts/tagDecoratorConfig.js",
                "~/scripts/video.js",

                "~/scripts/jquery.validate.js",
                "~/scripts/jquery.validate.unobtrusive.js",
                "~/scripts/jquery.validate.unobtrusive.chameleon.js",
                "~/scripts/jquery.validate.unobtrusive.acom.js",

                "~/scripts/tagManager/tagManager.js",
                "~/scripts/tagManager/tagConfig.js",
                "~/scripts/tagManager/media/*.js",
            };

            var cdnBundle = new ScriptBundle(Path.CdnPath).Include(items);
            cdnBundle.CdnPath = cdnBundle.GetCdnPath(this.cdnConfiguration.GetCurrentCdnPrefix());

            var localBundle = new ScriptBundle(Path.LocalPath).Include(items);

            bundleCollection.Add(cdnBundle);
            bundleCollection.Add(localBundle);
        }
    }
}