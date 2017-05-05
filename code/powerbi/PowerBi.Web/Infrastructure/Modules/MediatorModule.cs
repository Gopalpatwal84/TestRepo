using Autofac;
using Mediation;
using Mediation.Handlers;
using Mediation.Interceptors;
using Mediation.Queues;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace PowerBI.Web.Infrastructure.Modules
{
    public class MediatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => x.ResolveKeyed<CloudStorageAccount>(ServiceSettingsModule.WebjobsStorage).CreateCloudQueueClient())
                .Keyed<CloudQueueClient>(QueuedMediator.CloudQueueClientTag)
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .AsClosedTypesOf(typeof(IHandleCommand<>));

            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .AsClosedTypesOf(typeof(IHandleRequest<,>));

            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .AsClosedTypesOf(typeof(IHandleCompetingEvent<>));

            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(x => x.IsAssignableTo<IOutboundInterceptor>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<QueuedMediator>()
               .As<IMediator>()
               .InstancePerLifetimeScope();

            builder.RegisterType<AutofacResolver>()
                .As<IResolver>()
                .InstancePerLifetimeScope();
        }
    }
}