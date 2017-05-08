namespace BusinessPlatform.Jobs.CommonModules
{
    using Acom.IO;
    using Acom.IO.Json;
    using Autofac;
    using BusinessPlatform.Entities;
    using BusinessPlatform.Json;

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
