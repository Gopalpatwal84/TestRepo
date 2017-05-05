namespace PowerBI.Jobs.Forms
{
    using System.IO;
    using System.Threading.Tasks;
    using Autofac;
    using Mediation.Webjobs;
    using Microsoft.Azure.WebJobs;
    using PowerBI.Jobs.CommonModules;
    using PowerBI.Messages.Commands;

    public class Program
    {
        private static IContainer container;

        public static void Main()
        {
            container = InitializeContainer();
            container.Resolve<JobHost>().RunAndBlock();
        }

        public static Task ProcessNewsletterForm([QueueTrigger("%" + nameof(RegistrationRequestFormMessage) + "%")] RegistrationRequestFormMessage message, TextWriter logger)
        {
            return container.HandleCommand(message, logger);
        }

        public static Task ProcessSignupForm([QueueTrigger("%" + nameof(SignUpRequestFormMessage) + "%")] SignUpRequestFormMessage message, TextWriter logger)
        {
            return container.HandleCommand(message, logger);
        }

        public static Task ProcessDownloadForm([QueueTrigger("%" + nameof(RegistrationRequestFormMessage) + "%")] RegistrationRequestFormMessage message, TextWriter logger)
        {
            return container.HandleCommand(message, logger);
        }

        public static Task ProcessSupportForm([QueueTrigger("%" + nameof(SupportTicketFormMessage) + "%")] SupportTicketFormMessage message, TextWriter logger)
        {
            return container.HandleCommand(message, logger);
        }

        private static IContainer InitializeContainer()
        {
            var builder = new ContainerBuilder();
            builder.InstallCommonPowerBiModules();
            return builder.Build();
        }
    }
}
