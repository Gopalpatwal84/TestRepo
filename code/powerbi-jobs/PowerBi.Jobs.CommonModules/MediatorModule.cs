using System.Linq;
using System.Reflection;
using Autofac;
using Mediation;
using Mediation.Handlers;
using Mediation.Interceptors;
using Mediation.Queues;
using Mediation.Webjobs.Interceptor;
using PowerBI.Jobs.CommonModules.Instrumentation;
using Module = Autofac.Module;

namespace PowerBI.Jobs.CommonModules
{
    public class MediatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                .AsClosedTypesOf(typeof(IHandleCommand<>));

            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                .AsClosedTypesOf(typeof(IHandleRequest<,>));

            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                .AsClosedTypesOf(typeof(IHandleCompetingEvent<>));
            
            builder.RegisterType<MessageInstrumentationService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly(), this.ThisAssembly)
                .Where(x => x.IsAssignableTo<IInboundInterceptor>() || x.IsAssignableTo<IOutboundInterceptor>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<QueuedMediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AutofacResolver>()
                .As<IResolver>()
                .InstancePerLifetimeScope();

            builder.Register(x =>
            {
                var context = x.Resolve<IComponentContext>();
                QueuedMediator.Factory factory = (account, slot) => new QueuedMediator(account, slot, () => context.Resolve<IResolver>());
                return factory;
            }).InstancePerDependency();

            builder.RegisterType<MediatorBroadcaster>();
        }
    }
}
