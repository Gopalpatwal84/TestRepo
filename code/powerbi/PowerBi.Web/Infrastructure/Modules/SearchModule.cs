using Acom.Search.Client;
using Acom.Search.Client.Documents;
using Autofac;
using PowerBI.Search;
using PowerBI.Search.Documents;

namespace PowerBI.Web.Infrastructure.Modules
{
    public class SearchModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SearchClientWithFailover>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<SearchVersion>().AsImplementedInterfaces().SingleInstance();
            SearchEntryTypes.RegisterAssembly(typeof(PowerBiEntry).Assembly);
        }
    }
}