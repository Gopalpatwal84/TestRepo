using System.IO;
using System.Threading.Tasks;
using Autofac;
using Mediation.Webjobs;
using Microsoft.Azure.WebJobs;
using PowerBI.Jobs.CommonModules;
using PowerBI.Messages.Refresh;

namespace PowerBI.Jobs.BlogPump
{
    public class BlogPumpProgram
    {
        private static IContainer s_container;

        static void Main()
        {
            s_container = InitalizeContainer();

            s_container.Resolve<JobHost>()
                .RunAndBlock();

            s_container.Dispose();
        }

        public static Task ProcessRefreshFullMessage([QueueTrigger("%" + nameof(RefreshFullBlogCommand) + "%")] RefreshFullBlogCommand command, TextWriter log)
        {
            return s_container.HandleCommand(command, log);
        }

        [Singleton(KnownWebjobSettings.SlotNameField, Mode = SingletonMode.Listener, Account = KnownStorageKeys.GlobalSingleton)]
        public static Task ProcessRefreshFullTimer([TimerTrigger(KnownTimerConfigs.OnTheHour, RunOnStartup = true)] TimerInfo timerInfo, TextWriter log)
        {
            return s_container.HandleCommand(new RefreshFullBlogCommand { ReadSource = ReadSource.Remote }, log);
        }

        static IContainer InitalizeContainer()
        {
            var builder = new ContainerBuilder();
            builder.InstallCommonPowerBiModules();
            return builder.Build();
        }
    }
}
