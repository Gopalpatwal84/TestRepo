namespace BusinessPlatform.Jobs.CommonModules
{
    using System.Linq;
    using Acom.Configuration;
    using Acom.Configuration.Services;
    using Acom.Configuration.Services.Specialized;
    using Autofac;
    using Mediation.Queues;
    using Mediation.Webjobs;
    using Microsoft.Azure.WebJobs;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;

    public class StorageModule : Module
    {
        private static object webjobsStorage = new object();

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => x.Resolve<ServiceSettings>()
                .ServiceGroups.First(group => group.Name == "BusinessPlatform-jobs")
                .StorageConnections
                .ForRegionOrNull(KnownAntaresVariables.RegionName))
                .AsSelf();

            builder.Register(x => new JobHostConfiguration
                {
                    NameResolver = new AttributeQueueResolver(),
                    StorageConnectionString = x.Resolve<StorageConnectionInfo>().ConnectionString,
                    DashboardConnectionString = x.Resolve<StorageConnectionInfo>().ConnectionString,
                })
                .OnActivated(x => x.Instance.UseTimers())
                .SingleInstance();

            builder.Register(x => new JobHost(x.Resolve<JobHostConfiguration>()))
                .SingleInstance();

            builder.Register(x => CloudStorageAccount.Parse(x.Resolve<StorageConnectionInfo>().ConnectionString))
                  .Keyed<CloudStorageAccount>(webjobsStorage)
                  .SingleInstance();

            builder.Register(x => x.ResolveKeyed<CloudStorageAccount>(webjobsStorage).CreateCloudQueueClient())
                .Keyed<CloudQueueClient>(webjobsStorage)
                .Keyed<CloudQueueClient>(QueuedMediator.CloudQueueClientTag)
                .InstancePerLifetimeScope();

            builder.Register(x => x.ResolveKeyed<CloudStorageAccount>(webjobsStorage).CreateCloudBlobClient())
                .Keyed<CloudBlobClient>(webjobsStorage)
                .InstancePerLifetimeScope()
                .OnActivated(x => x.Instance.DefaultRequestOptions.RetryPolicy = new ExponentialRetry());
        }
    }
}
