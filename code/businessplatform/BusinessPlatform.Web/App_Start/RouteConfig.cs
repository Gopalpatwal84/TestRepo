namespace BusinessPlatform.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public const string CultureRegex = @"^[a-z]{2}-[a-z]{2}$|^[a-z]{2}$";

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;
            routes.AppendTrailingSlash = true;

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
