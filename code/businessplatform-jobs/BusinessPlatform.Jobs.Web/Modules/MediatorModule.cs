﻿namespace BusinessPlatform.Jobs.Web.Modules
{
    using Acom.Configuration.Services;
    using Autofac;
    using BusinessPlatform.Jobs.Web.Services;
    using Mediation;
    using Mediation.Queues;

    public class MediatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ServiceSettings>()
                .As<ServiceSettings>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MessageBroadcastService>()
                .As<MessageBroadcastService>()
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
            });
        }
    }
}