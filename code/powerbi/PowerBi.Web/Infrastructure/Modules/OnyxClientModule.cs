using Autofac;
using Onyx.Client;
using Serilog;

namespace PowerBI.Web.Infrastructure.Modules
{
    public class OnyxClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Onyx.Client.Configuration>()
                .SingleInstance();

            builder.RegisterType<AuthToken>()
                .SingleInstance();

            builder.RegisterType<OnyxRequestClient>()
                .As<IOnyxRequestClient>()
                .WithParameter(new TypedParameter(typeof(ILogger), Log.Logger))
                .InstancePerLifetimeScope();
        }
    }
}