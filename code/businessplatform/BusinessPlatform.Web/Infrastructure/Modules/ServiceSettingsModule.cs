namespace BusinessPlatform.Web.Infrastructure.Modules
{
    using System.Linq;
    using Acom.Configuration;
    using Acom.Configuration.Services;
    using Autofac;
    using Microsoft.WindowsAzure.Storage;

    public class ServiceSettingsModule : Module
    {
        public static object WebjobsStorage = new object();

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ServiceSettings>().SingleInstance();

            builder.Register(x =>
            {
                var connections = x.Resolve<ServiceSettings>()
                .ServiceGroups
                .First(group => group.Name == "BusinessPlatform-jobs")
                .StorageConnections;

                return CloudStorageAccount.Parse(
                    (connections.ForRegionOrNull(KnownAntaresVariables.RegionName) ??
                    connections.Default()).ConnectionString);
            }).Keyed<CloudStorageAccount>(WebjobsStorage).SingleInstance();
        }
    }
}