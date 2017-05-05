using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using PowerBI.Json.Modules;

namespace PowerBI.Web
{
    public static class ContainerConfig
    {
        public static IContainer RegisterIoC()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();

            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterFilterProvider();

            var config = GlobalConfiguration.Configuration;
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly(), 
                typeof(Acom.Mvc.PagesApi.PagesApiController).Assembly, 
                typeof(Onyx.Mvc.RobotsApi.RobotsApiController).Assembly);
            builder.RegisterWebApiFilterProvider(config);

            builder.RegisterAssemblyModules(typeof(MvcApplication).Assembly);
            builder.RegisterAssemblyModules(typeof(JsonModule).Assembly);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            return container;
        }
    }
}