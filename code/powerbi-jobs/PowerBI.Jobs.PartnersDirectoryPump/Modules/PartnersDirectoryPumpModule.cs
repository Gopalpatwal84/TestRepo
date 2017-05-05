using Autofac;
using Onyx.Client;
using PowerBI.Jobs.PartnersDirectoryPump.Features;

namespace PowerBI.Jobs.PartnersDirectoryPump.Modules
{
    public class PartnersDirectoryPumpModule : Module
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

            builder.RegisterType<PartnersDirectoryReader>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}