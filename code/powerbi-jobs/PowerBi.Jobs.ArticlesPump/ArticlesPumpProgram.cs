using System.IO;
using System.Threading.Tasks;
using Autofac;
using Mediation.Webjobs;
using Microsoft.Azure.WebJobs;
using PowerBI.Jobs.CommonModules;
using PowerBI.Messages.Refresh;

namespace PowerBI.Jobs.ArticlesPump
{
    public class ArticlesPumpProgram
    {
        private static IContainer container;

        public static void Main(string[] args)
        {
            container = InitializeContainer();

            container.Resolve<JobHost>()
                .RunAndBlock();

            container.Dispose();
        }

        [Singleton(KnownWebjobSettings.SlotNameField, Mode = SingletonMode.Listener)]
        public static Task ProcessTimer([TimerTrigger(KnownTimerConfigs.OnTheHour, RunOnStartup = true)] TimerInfo timerInfo, TextWriter log)
        {
            return container.HandleCommand(new RefreshArticlesCommand(), log);
        }

        public static Task ProcessMessage([QueueTrigger("%RefreshArticlesCommand%")] RefreshArticlesCommand command, TextWriter log)
        {
            return container.HandleCommand(command, log);
        }

        private static IContainer InitializeContainer()
        {
            var builder = new ContainerBuilder();
            builder.InstallCommonPowerBiModules();
            return builder.Build();
        }
    }
}
