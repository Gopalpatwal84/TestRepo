namespace PowerBI.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using PowerBI.Web.Infrastructure.Helpers;

    public static class RouteConfig
    {
        public const string CultureRegex = @"^[a-z]{2}-[a-z]{2}$|^[a-z]{2}$";

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;
            routes.AppendTrailingSlash = true;

            // Ignore rules for reverse proxy
            routes.IgnoreRoute("{*pathInfo}", new { pathInfo = new ReverseProxyPages() });
            routes.IgnoreRoute("{culture}/{*pathInfo}", new { culture = CultureRegex, pathInfo = new ReverseProxyPages() });
            routes.IgnoreRoute("js/{*pathInfo}");
            routes.IgnoreRoute("styles/{*pathInfo}");
            routes.IgnoreRoute("images/{*pathInfo}");
            routes.IgnoreRoute("blob/{*pathInfo}");
            routes.IgnoreRoute("partners/{pathInfo}");
            routes.IgnoreRoute("sitemap/{culture}", new { culture = CultureRegex });

            // Ignore rule for mediahandler
            routes.IgnoreRoute("mediahandler/{*pathInfo}");

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                "Home",
                "{culture}",
                new { controller = "Home", action = "Index" },
                new { culture = CultureRegex });

            routes.MapRoute(
                "Page",
                "{culture}/{*viewPath}",
                new { controller = "Site", action = "Page" },
                new { culture = CultureRegex });

            routes.MapRoute(
                "PageNoCulture",
                "{*viewPath}",
                new { controller = "Site", action = "Page" });
        }
    }
}