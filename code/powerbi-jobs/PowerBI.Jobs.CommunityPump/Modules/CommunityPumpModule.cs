using Autofac;
using PowerBI.Jobs.CommunityPump.Reader;

namespace PowerBI.Jobs.CommunityPump.Modules
{
    public class CommunityPumpModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LithiumReader>().InstancePerLifetimeScope();
        }
    }
}
