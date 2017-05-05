namespace PowerBI.Jobs.DocumentationFeedback
{
    using System.IO;
    using System.Threading.Tasks;
    using Autofac;
    using Mediation.Webjobs;
    using Microsoft.Azure.WebJobs;
    using PowerBI.Jobs.CommonModules;
    using PowerBI.Messages.Commands;

    public class DocumentationFeedbackProgram
    {
        private static IContainer container;

        public static void Main()
        {
            container = InitializeContainer();
            container.Resolve<JobHost>().RunAndBlock();
        }

        public static Task ProcessDocumentationFeedback([QueueTrigger("%ArticleFeedbackMessage%")] ArticleFeedbackMessage articleFeedback, TextWriter logger)
        {
            return container.HandleCommand(articleFeedback, logger);
        }

        private static IContainer InitializeContainer()
        {
            var builder = new ContainerBuilder();
            builder.InstallCommonPowerBiModules();

            return builder.Build();
        }
    }
}
