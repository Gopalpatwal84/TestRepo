namespace PowerBI.Jobs.Forms.PartnersInvitations.Modules
{
    using Autofac;
    using Onyx.Client;
    using PowerBI.Jobs.Forms.PartnersInvitations.Services;

    public class JobModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Onyx.Client.Configuration>()
                .SingleInstance();

            builder.RegisterType<AuthToken>()
                .SingleInstance();

            builder.RegisterType<OnyxRequestClient>()
                .As<IOnyxRequestClient>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PartnersInvitationService>().AsSelf();
        }
    }
}