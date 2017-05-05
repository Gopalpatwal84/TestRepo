using System.IO;
using System.Threading.Tasks;
using Autofac;
using Mediation.Webjobs;
using Microsoft.Azure.WebJobs;
using PowerBI.Jobs.CommonModules;
using PowerBI.Messages.Refresh;

namespace PowerBI.Jobs.SupportTopicsPump
{
    public class Program
    {
        private static IContainer container;

        public static void Main()
        {
            container = InitalizeContainer();
            container.Resolve<JobHost>()
                .RunAndBlock();

            container.Dispose();
        }

        [Singleton(KnownWebjobSettings.SlotNameField, Mode = SingletonMode.Listener)]
        public static Task ProcessTimer([TimerTrigger(KnownTimerConfigs.AtMidnight, RunOnStartup = true)] TimerInfo timerInfo, TextWriter log)
        {
            return container.HandleCommand(new RefreshSupportTopicsCommand { RunFullUpdate = true, }, log);
        }

        public static Task ProcessMessage([QueueTrigger("%RefreshSupportTopicsCommand%")] RefreshSupportTopicsCommand command, TextWriter log)
        {
            return container.HandleCommand(command, log);
        }

        public static IContainer InitalizeContainer()
        {
            var builder = new ContainerBuilder();
            builder.InstallCommonPowerBiModules();
            return builder.Build();
        }
    }
}
