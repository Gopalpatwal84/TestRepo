namespace BusinessPlatform.Jobs.CommonModules
{
    using System.Reflection;
    using Acom.Configuration.Services;
    using Acom.Configuration.Settings;
    using Autofac;

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
