using System;
using System.Web.Mvc;
using PowerBI.Geo;
using PowerBI.Web.Models.Pages;
using System.Threading.Tasks;

namespace PowerBI.Web.Infrastructure.Helpers
{
    public class CloudPreferenceFilter : ActionFilterAttribute
    {
        private static string _userGeoCookie = "_userGeoInfoCookie";
        private static DateTime _userGeoCookieValidDuration = DateTime.UtcNow.AddHours(24);
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var service = DependencyResolver.Current.GetService<IGeoLocationService>();

            var cloud = filterContext.HttpContext.Request.QueryString["cloud"];
            // Avoiding Orion api call if information avalaible either in query string or cookie
            cloud = string.IsNullOrEmpty(cloud) ? filterContext.HttpContext.GetCookie(_userGeoCookie) : cloud;

            if (string.IsNullOrEmpty(cloud))
            {
                var country =
                    Task.Factory.StartNew(() =>
                        service.GetCountryOrDefault(filterContext.HttpContext.Request.UserIPAddress()))
                            .Unwrap()
                            .GetAwaiter()
                            .GetResult();

                cloud = country.Slug;
            }

            if (!string.IsNullOrEmpty(cloud))
            {
                //Setting another cookie to capture geolocation for all countries in order to avoid unnecessary Orion API calls that tax load in production
                //Expiry set for 24 hours to allow for possible relocation of users
                filterContext.HttpContext.SetCookie(_userGeoCookie, cloud, _userGeoCookieValidDuration);

                if (KnownClouds.ValidClouds.Contains(cloud.ToLowerInvariant()))
                {
                    filterContext.HttpContext.SetCloudPreference(cloud);
                }
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as ViewResult;
            if (result?.Model is ViewMetadataModel)
            {
                var viewModel = result.Model as ViewMetadataModel;
                viewModel.IsChinaCloud = filterContext.HttpContext.IsCloudPreference(KnownClouds.China);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
