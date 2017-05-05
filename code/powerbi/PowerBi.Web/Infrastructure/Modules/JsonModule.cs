using System.Web.Hosting;
using Acom.IO.FileReaders;
using Acom.IO.Json;
using Acom.Mvc.Contracts;
using Acom.Mvc.PagesApi;
using Autofac;

namespace PowerBI.Web.Infrastructure.Modules
{
    public class JsonRequestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JsonRequestHelper>().AsImplementedInterfaces();
            builder.Register(x => new FileReader(HostingEnvironment.MapPath("~/"))).AsSelf().SingleInstance();
            builder.Register(x => new PagesJsonLoader(typeof(Resources.Pages.Index).Assembly))
                .As<JsonLoader<Page>>()
                .SingleInstance();
            builder.RegisterType<JsonRepository<Page>>().AsImplementedInterfaces().SingleInstance();
        }
    }
}