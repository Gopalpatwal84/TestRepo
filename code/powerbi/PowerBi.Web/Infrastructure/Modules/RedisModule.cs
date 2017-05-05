using System.Linq;
using Acom.Configuration;
using Acom.Configuration.SecondaryRegions;
using Acom.Configuration.Services;
using Acom.IO.Entities;
using Acom.Redis;
using Autofac;
using Mediation;
using PowerBI.Entities;
using PowerBI.Json.Modules;

namespace PowerBI.Web.Infrastructure.Modules
{
    public class RedisModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x =>
            {
                var connection = x.Resolve<ServiceSettings>()
                    .ServiceGroups.First(group => group.Name == KnownDeploymentVariables.DeploymentGroup)
                    .RedisConnections;

                return connection.GetServiceFailover(config => new RedisFailover(x.Resolve<IMediator>(), config));
            }).SingleInstance();

            var allEntities = typeof(Culture).Assembly
                .GetExportedTypes()
                .Where(x => x.IsAssignableTo<IHaveSlug>())
                .ToArray();
            var jsonTypes = JsonModule.GetJsonEntityTypes();

            var redisRepositoryTypes = allEntities
                .Except(jsonTypes)
                .Select(x => typeof(RedisRepository<>).MakeGenericType(x))
                .ToArray();

            builder.RegisterTypes(redisRepositoryTypes).AsImplementedInterfaces().SingleInstance();
        }
    }
}