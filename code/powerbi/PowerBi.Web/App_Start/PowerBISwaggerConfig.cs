using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using Acom.Swagger;
using Swashbuckle.Application;

namespace PowerBI.Web
{
    public class PowerBiSwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config
                .EnableSwagger(docsConfig =>
                {
                    docsConfig.ApplyCommonCustomizations(vc =>
                    {
                        vc.Version("latest", "Power BI Services API");
                        vc.Version("v1", "Power BI Services API");
                    });


                    docsConfig.IncludeXmlComments(HostingEnvironment.MapPath("~/Acom.Mvc.PagesApi.XML"));
                    docsConfig.IncludeXmlComments(HostingEnvironment.MapPath("~/Acom.Mvc.Contracts.XML"));

                })
                .EnableSwaggerUi(uiConfig =>
                {
                    uiConfig.ApplyCommonUiCustomizations();
                });
        }
    }
}
