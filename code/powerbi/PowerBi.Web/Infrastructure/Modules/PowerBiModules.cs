using System.Web.Hosting;
using Acom.Configuration.Settings;
using Acom.IO.FileReaders;
using Acom.Mvc.Bundles;
using Acom.Mvc.Cdn;
using Autofac;
using PowerBI.Web.Infrastructure.Helpers;
using Serilog;

namespace PowerBI.Web.Infrastructure.Modules
{
    public class PowerBiModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CultureHelper>().AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IBundle>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly, typeof(CdnConfiguration).Assembly)
                .AssignableTo<ConfigurationSettings>()
                .SingleInstance();

            builder.Register(x => new FileReader(HostingEnvironment.MapPath("~/"))).AsSelf().SingleInstance();

            builder.Register(x => Log.Logger).As<ILogger>();

            builder.RegisterType<SearchHelper>().AsSelf();
        }
    }
}