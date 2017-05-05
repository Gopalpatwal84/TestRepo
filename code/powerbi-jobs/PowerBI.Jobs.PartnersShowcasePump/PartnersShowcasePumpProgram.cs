using System.IO;
using System.Threading.Tasks;
using Autofac;
using Mediation.Webjobs;
using Microsoft.Azure.WebJobs;
using PowerBI.Jobs.CommonModules;
using PowerBI.Messages.Refresh;

namespace PowerBI.Jobs.PartnersShowcasePump
{
    public class PartnersShowcasePumpProgram
    {
        private static IContainer s_container;

        static void Main()
        {
            s_container = InitalizeContainer();

            s_container.Resolve<JobHost>()
                .RunAndBlock();

            s_container.Dispose();
        }

        [Singleton(KnownWebjobSettings.SlotNameField, Mode = SingletonMode.Listener, Account = KnownStorageKeys.GlobalSingleton)]
        public static Task ProcessTimer([TimerTrigger(KnownTimerConfigs.OnTheHour, RunOnStartup = true)] TimerInfo timerInfo, TextWriter log)
        {
            return s_container.HandleCommand(new RefreshPartnersShowcaseCommand { Source = ReadSource.Remote, }, log);
        }

        public static Task ProcessMessage([QueueTrigger("%RefreshPartnersShowcaseCommand%")] RefreshPartnersShowcaseCommand command, TextWriter log)
        {
            return s_container.HandleCommand(command, log);
        }

        static IContainer InitalizeContainer()
        {
            var builder = new ContainerBuilder();
            builder.InstallCommonPowerBiModules();
            return builder.Build();
        }
    }
}
