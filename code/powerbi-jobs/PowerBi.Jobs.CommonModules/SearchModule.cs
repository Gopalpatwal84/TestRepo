using Acom.Search.Client;
using Acom.Search.Client.Documents;
using Acom.Search.Queue;
using Autofac;
using PowerBI.Search;
using PowerBI.Search.Documents;

namespace PowerBI.Jobs.CommonModules
{
    public class SearchModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterSearchModules();
            builder.RegisterSearchQueueModules();

            builder.RegisterType<SearchVersion>()
                .AsImplementedInterfaces();

            builder.Register(x => new AttributeEntityToEntryResolver(typeof(PowerBiEntry).Assembly))
                .As<EntityToEntryResolver>()
                .SingleInstance()
                .AutoActivate();
        }
    }
}