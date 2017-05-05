using Autofac;
using Onyx.Client;

namespace PowerBI.Jobs.BlogPump.Modules
{
    public class BlogPumpModule : Module
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
        }
    }
}
