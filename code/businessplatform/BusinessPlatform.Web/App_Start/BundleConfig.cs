namespace BusinessPlatform.Web
{
    using System.Collections.Generic;
    using System.Web.Optimization;
    using Acom.Mvc.Bundles;
    using Autofac;

    public static class BundleConfig
    {
        public static void ConfigureBundles(ILifetimeScope lifetimeScope, BundleCollection bundleCollection)
        {
            BundleTable.Bundles.UseCdn = true;
            var bundles = lifetimeScope.Resolve<IEnumerable<IBundle>>();
            foreach (var bundle in bundles)
            {
                bundle.Register(bundleCollection);
            }
        }
    }
}