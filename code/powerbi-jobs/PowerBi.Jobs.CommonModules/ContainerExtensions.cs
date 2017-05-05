using System;
using System.Reflection;
using Acom.Configuration;
using Acom.Configuration.Services.Specialized;
using Autofac;
using Serilog;

namespace PowerBI.Jobs.CommonModules
{
    public static class ContainerExtensions
    {
        public static void InstallCommonPowerBiModules(this ContainerBuilder builder)
        {
            Console.WriteLine("Running in region: '{0}'", KnownAntaresVariables.RegionName);

            builder.RegisterAssemblyModules(Assembly.GetEntryAssembly());
            builder.RegisterAssemblyModules(typeof(LoggingModule).Assembly);
        }

        public static T Dump<T>(this T info, ILogger logger) where T : IConnectionInfo
        {
            if (info == null) logger.Error("Error, connection info was not found.");
            else
            {
                logger.Information("Found {IConnectionInfo} for region: {RegionName}", typeof(T).Name, info.Region);
                var connectionInfo = info as ConnectionInfo;
                if (connectionInfo != null)
                    logger.Information("Found host: {ConnectionHost}", connectionInfo.Host);
            }
            return info;
        }
    }
}
