namespace PowerBI.Jobs.Web
{
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            SetJsonMediaTypeFormatter(config);
        }

        public static void SetJsonMediaTypeFormatter(HttpConfiguration config)
        {
            if (config != null)
            {
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
}