namespace PowerBI.Jobs.DocumentationFeedback.Modules
{
    using Autofac;
    using Data.Context;
    using PowerBI.Jobs.DocumentationFeedback.Data;

    public class DocumentationFeedbackModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DocumentationFeedbackDbContext>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<DocumentationFeedbackDbDataService>().AsSelf();
        }
    }
}
