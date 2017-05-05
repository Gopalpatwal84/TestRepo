namespace PowerBI.Jobs.Forms.PartnersInvitations
{
    using System.IO;
    using System.Threading.Tasks;
    using Autofac;
    using Mediation.Webjobs;
    using Microsoft.Azure.WebJobs;
    using PowerBI.Messages.Commands;
    using PowerBI.Jobs.CommonModules;

    public class Program
    {
        private static IContainer container;

        public static void Main()
        {
            container = InitializeContainer();
            container.Resolve<JobHost>().RunAndBlock();
        }

        public static Task ProcessMessage([QueueTrigger("%PartnersInvitationFormMessage%")] PartnersInvitationFormMessage message, TextWriter logger)
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
