using Autofac;
using PowerBI.Geo;

namespace PowerBI.Web.Infrastructure.Modules
{
    public class GeoModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GeoLocationService>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
