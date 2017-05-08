namespace BusinessPlatform.Jobs.Forms
{
    using System.IO;
    using System.Threading.Tasks;
    using Autofac;
    using BusinessPlatform.Jobs.CommonModules;
    using BusinessPlatform.Messages.Commands;
    using Mediation.Webjobs;
    using Microsoft.Azure.WebJobs;

    public class Program
    {
        private static IContainer container;

        public static void Main()
        {
            // TODO: Since this webjob is not in use we are commenting it. Will revisit as it is required.
            //// container = InitializeContainer();
            //// container.Resolve<JobHost>().RunAndBlock();
        }

        public static Task ProcessForm([QueueTrigger("%SignUpRequestFormMessage%")] SignUpRequestFormMessage message, TextWriter logger)
        {
            return container.HandleCommand(message, logger);
        }

        private static IContainer InitializeContainer()
        {
            var builder = new ContainerBuilder();
            builder.InstallCommonBusinessPlatformModules();
            return builder.Build();
        }
    }
}
