namespace BusinessPlatform.Web.Infrastructure.Modules
{
    using System.Web.Hosting;
    using Acom.Configuration.Settings;
    using Acom.IO.FileReaders;
    using Acom.Mvc.Bundles;
    using Autofac;
    using BusinessPlatform.Web.Infrastructure.Helpers;

    public class BusinessPlatformModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CultureHelper>().AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IBundle>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<ConfigurationSettings>()
                .SingleInstance();

            builder.Register(x => new FileReader(HostingEnvironment.MapPath("~/"))).AsSelf().SingleInstance();
        }
    }
}