using System.Web.Configuration;
using System.Web.Hosting;
using Serilog;
using Serilog.Events;
using System;
using Acom.Mvc.Logging;

namespace PowerBI.Web
{
    public static class LoggerConfig
    {
        public static void ConfigureLogging()
        {
            var fileBuffer = string.Equals("true", WebConfigurationManager.AppSettings["SeqAllowFileBuffer"], StringComparison.OrdinalIgnoreCase) ? HostingEnvironment.MapPath("~/App_Data/Logs/Log") : null;
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Trace()
                .WriteTo.Seq(WebConfigurationManager.AppSettings["SeqServer"], apiKey: WebConfigurationManager.AppSettings["SeqServerApiKey"], bufferBaseFilename: fileBuffer, restrictedToMinimumLevel: LogEventLevel.Information)
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Application", "PowerBI.Web")
                .Enrich.WithProperty("AppDomain", AppDomain.CurrentDomain)
                .Enrich.WithProperty("RuntimeVersion", Environment.Version)
                .Enrich.With<AntaresEnricher>()
                .Enrich.With<DeploymentEnricher>()
                .Enrich.With<WebEnricher>()
                .Enrich.FromLogContext()
                .CreateLogger();
        }
    }
}