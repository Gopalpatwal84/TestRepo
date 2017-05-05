using Autofac;
using Onyx.Client;
using PowerBI.Jobs.PartnersShowcasePump.Features;

namespace PowerBI.Jobs.PartnersShowcasePump.Modules
{
    public class PartnersShowcasePumpModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Configuration>()
                .SingleInstance();

            builder.RegisterType<AuthToken>()
                .SingleInstance();

            builder.RegisterType<OnyxRequestClient>()
                .As<IOnyxRequestClient>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PartnersShowcaseReader>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}