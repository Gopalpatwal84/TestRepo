using System.Reflection;
using Acom.Configuration.Services;
using Acom.Configuration.Settings;
using Autofac;

namespace PowerBI.Jobs.CommonModules
{
    public class ConfigurationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ServiceSettings>()
                .AsSelf()
                .SingleInstance()
                .AutoActivate();

            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
               .AssignableTo<ConfigurationSettings>()
               .AsSelf()
               .SingleInstance();
        }
    }
}
