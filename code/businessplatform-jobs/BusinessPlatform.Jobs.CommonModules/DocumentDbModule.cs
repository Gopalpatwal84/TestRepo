﻿namespace BusinessPlatform.Jobs.CommonModules
{
    using System.Linq;
    using Acom.Configuration.Services;
    using Acom.DocumentDb;
    using Autofac;
    using Mediation;
    using Serilog;

    public class DocumentDbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => new DocumentDbClient(
                    x.Resolve<ServiceSettings>()
                    .ServiceGroups.First(group => group.Name == "BusinessPlatform-jobs")
                    .DocumentDbConnections.Default() // single region
                    .Dump(x.Resolve<ILogger>()),
                    x.Resolve<IMediator>()))
               .As<IDocumentDbClient>()
               .InstancePerLifetimeScope();
        }
    }
}
