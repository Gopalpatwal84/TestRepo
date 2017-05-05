using Autofac;
using PowerBI.Support.Helpers;
using PowerBI.Support.Services;

namespace PowerBI.Web.Infrastructure.Modules
{
    public class JobModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PowerBI.Support.AuthConfiguration>()
                .SingleInstance();
            builder.RegisterType<PowerBI.Support.Configuration>()
                .SingleInstance();

            builder.RegisterType<AuthToken>()
                .SingleInstance();

            builder.RegisterType<MetisService>().AsSelf();
        }
    }
}