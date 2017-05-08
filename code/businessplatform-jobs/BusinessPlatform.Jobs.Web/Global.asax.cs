namespace BusinessPlatform.Jobs.Web
{
    using System;
    using System.Reflection;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Hosting;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Acom.Configuration;
    using Acom.Mvc.Bundles;
    using Autofac;
    using Autofac.Integration.WebApi;
    using BusinessPlatform.Jobs.Web;
    using BusinessPlatform.Jobs.Web.Helpers;
    using BusinessPlatform.Jobs.Web.Modules;
    using Serilog;
    using Serilog.Events;

    public class MvcApplication : System.Web.HttpApplication
    {
        static MvcApplication()
        {
            var version = VersionHelper.GetVersionInfo();
            var fileBuffer = string.Equals("true", WebConfigurationManager.AppSettings["SeqAllowFileBuffer"], StringComparison.OrdinalIgnoreCase) ? HostingEnvironment.MapPath("~/App_Data/Logs/Log") : null;
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Trace()
                .WriteTo.Seq(WebConfigurationManager.AppSettings["SeqServer"], apiKey: WebConfigurationManager.AppSettings["SeqServerApiKey"], bufferBaseFilename: fileBuffer, restrictedToMinimumLevel: LogEventLevel.Information)
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Application", "BusinessPlatform.Jobs.Web")
                .Enrich.WithProperty("AppDomain", AppDomain.CurrentDomain)
                .Enrich.WithProperty("RuntimeVersion", Environment.Version)
                .Enrich.WithProperty("Region", KnownAntaresVariables.RegionName)
                .Enrich.WithProperty("SiteName", KnownAntaresVariables.SiteName)
                .Enrich.WithProperty("InstanceId", KnownAntaresVariables.InstanceId)
                .Enrich.WithProperty("SlotName", KnownSlots.SlotName)
                .Enrich.WithProperty("DeploymentVersion", version.Version)
                .Enrich.WithProperty("BranchName", version.BranchName)
                .Enrich.FromLogContext()
                .CreateLogger();

            Log.Warning("App Started");
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ConfigureIoc(GlobalConfiguration.Configuration);
        }

        private static void ConfigureIoc(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModule<MediatorModule>();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}