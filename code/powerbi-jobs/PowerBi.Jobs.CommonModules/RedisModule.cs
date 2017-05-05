using System.Linq;
using Acom.Configuration;
using Acom.Configuration.Services;
using Acom.Redis;
using Autofac;
using Mediation;
using Serilog;

namespace PowerBI.Jobs.CommonModules
{
    public class RedisModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Jobs don't fall back
            builder.Register(x => new RedisClient(
                x.Resolve<ServiceSettings>()
                    .ServiceGroups
                    .First(group => group.Name == KnownDeploymentVariables.DeploymentGroup)
                    .RedisConnections
                    .ForRegionOrNull(KnownAntaresVariables.RegionName)
                    .Dump(x.Resolve<ILogger>()),
                x.Resolve<IMediator>()))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
