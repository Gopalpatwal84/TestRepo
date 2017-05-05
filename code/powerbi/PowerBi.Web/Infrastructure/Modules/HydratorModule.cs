using Acom.Mvc;
using Autofac;

namespace PowerBI.Web.Infrastructure.Modules
{
    public class HydratorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .AsClosedTypesOf(typeof(IHydrateModel<>))
                .InstancePerLifetimeScope();
        }
    }
}