using System;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PowerBI.Web.Infrastructure.Helpers
{
    public class ReverseProxyPages : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            // negate the reverse proxy
            if(httpContext.Request.QueryString.Get("proxy") == "off")
            {
                return false;
            }

            var reverseProxyPagesWithCultureMapReader = new RewriteMapReader("ReverseProxyPagesWithCulture");
            var reverseProxyPagesWithoutCultureMapReader = new RewriteMapReader("ReverseProxyPagesWithoutCulture");

            if (values[parameterName] == null)
            {
                if (values["culture"] == null)
                {
                    return false;
                }

                return reverseProxyPagesWithCultureMapReader.Maps.ContainsKey("/")
                       || reverseProxyPagesWithoutCultureMapReader.Maps.ContainsKey("/");
            }

            var parameterValue = values[parameterName].ToString().ToLowerInvariant();
            if (values["culture"] != null)
            {
                return reverseProxyPagesWithCultureMapReader.Maps.Any(m => m.Value.ToLowerInvariant().Trim('/') == parameterValue.Trim('/'));
            }

            return reverseProxyPagesWithoutCultureMapReader.Maps.Any(m => m.Value.ToLowerInvariant().Trim('/') == parameterValue.Trim('/'));
        }
    }

    public class ReverseProxyKillswitch : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // negate the reverse proxy
            if (httpContext.Request.QueryString.Get("proxy") == "off")
            {
                return false;
            }

            return true;
        }
    }
}