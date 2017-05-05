using Acom.IO;
using Acom.IO.Json;
using Autofac;
using PowerBI.Entities;
using PowerBI.Json;

namespace PowerBI.Jobs.CommonModules
{
    public class JsonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyModules(typeof(CulturesJsonLoader).Assembly);

            builder.RegisterType(typeof(JsonRepository<Culture>))
                .As(typeof(IRepository<Culture>))
                .SingleInstance();
        }
    }
}
