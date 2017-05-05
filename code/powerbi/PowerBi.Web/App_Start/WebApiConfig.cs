using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Routing;
using Acom.Swagger;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PowerBI.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.EnableCors();

            // Web API routes
            var constraintResolver = new DefaultInlineConstraintResolver();
            constraintResolver.ConstraintMap.Add("between", typeof(VersionBetweenConstraint));
            config.MapHttpAttributeRoutes(constraintResolver, new ApiDirectRouteProvider());

            // Web API output caching
            SetJsonMediaTypeFormatter(config);
        }

        private static void SetJsonMediaTypeFormatter(HttpConfiguration config)
        {
            if (config == null)
            {
                return;
            }

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter
            {
                SerializerSettings =
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            });
        }
    }
}
