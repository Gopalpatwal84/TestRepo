using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using Acom.Configuration;
using Autofac;
using Serilog;
using Serilog.Events;

using Module = Autofac.Module;

namespace PowerBI.Jobs.CommonModules
{
    public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var fileBuffer = string.Equals("true", ConfigurationManager.AppSettings["SeqAllowFileBuffer"], StringComparison.OrdinalIgnoreCase) ? GetBaseLogPath() : null;

            builder.Register((x, c) =>
            {
                var textWriter = x.Resolve<TextWriter>();
                var config = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.TextWriter(textWriter, LogEventLevel.Verbose)
                    .Enrich.WithProperty("HandleMessageCorrelation", Guid.NewGuid());

                InitializeGlobalContext(config);

                if (KnownAntaresVariables.InstanceId == KnownAntaresVariables.Unknown)
                {
                    config.WriteTo.ColoredConsole(LogEventLevel.Verbose);
                }
                config.WriteTo.Seq(ConfigurationManager.AppSettings["SeqServer"], apiKey: ConfigurationManager.AppSettings["SeqServerApiKey"], bufferBaseFilename: fileBuffer);

                return config.CreateLogger();
            }).InstancePerLifetimeScope();

            var globalConfig = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(LogEventLevel.Verbose);
            InitializeGlobalContext(globalConfig);

            if (KnownAntaresVariables.InstanceId != KnownAntaresVariables.Unknown)
                globalConfig.WriteTo.Seq(ConfigurationManager.AppSettings["SeqServer"], apiKey: ConfigurationManager.AppSettings["SeqServerApiKey"], bufferBaseFilename: fileBuffer);

            Log.Logger = globalConfig.CreateLogger();
        }

        private static string GetBaseLogPath()
        {
            return Path.Combine(Environment.CurrentDirectory, "log");
        }

        public static LoggerConfiguration InitializeGlobalContext(LoggerConfiguration configuration)
        {
            return configuration.Enrich.WithMachineName()
                                .Enrich.WithProperty("AppDomain", AppDomain.CurrentDomain)
                                .Enrich.WithProperty("JobName", Assembly.GetEntryAssembly().GetName().Name.Split('.').Last())
                                .Enrich.WithProperty("RuntimeVersion", Environment.Version)
                                .Enrich.WithProperty("Region", KnownAntaresVariables.RegionName)
                                .Enrich.WithProperty("SiteName", KnownAntaresVariables.SiteName)
                                .Enrich.WithProperty("InstanceId", KnownAntaresVariables.InstanceId)
                                .Enrich.WithProperty("SlotName", KnownSlots.SlotName)
                                .Enrich.FromLogContext();
        }
    }
}
