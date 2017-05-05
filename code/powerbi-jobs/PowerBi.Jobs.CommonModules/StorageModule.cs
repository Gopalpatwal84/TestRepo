using System.Linq;
using Acom.Configuration;
using Acom.Configuration.Services;
using Acom.Configuration.Services.Specialized;
using Autofac;
using Mediation.Queues;
using Mediation.Webjobs;
using Mediation.Webjobs.Storage;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace PowerBI.Jobs.CommonModules
{
    public class StorageModule : Module
    {
        public static object WebjobsStorage = new object();
        public static object WebjobsStorageDefault = new object();

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => x.Resolve<ServiceSettings>()
                .ServiceGroups.First(group => group.Name == "pwrbi-jobs")
                .StorageConnections
                .ForRegionOrNull(KnownAntaresVariables.RegionName))
                .AsSelf();

            builder.Register(x => x.Resolve<ServiceSettings>()
                .ServiceGroups.First(group => group.Name == "pwrbi-jobs")
                .StorageConnections
                .Default())
                .Keyed<StorageConnectionInfo>(WebjobsStorageDefault);

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
                  .Keyed<CloudStorageAccount>(WebjobsStorage)
                  .SingleInstance();

            builder.Register(x => CloudStorageAccount.Parse(x.ResolveKeyed<StorageConnectionInfo>(WebjobsStorageDefault).ConnectionString))
                  .Keyed<CloudStorageAccount>(WebjobsStorageDefault)
                  .SingleInstance();

            builder.Register(x => x.ResolveKeyed<CloudStorageAccount>(WebjobsStorage).CreateCloudQueueClient())
                .Keyed<CloudQueueClient>(WebjobsStorage)
                .Keyed<CloudQueueClient>(QueuedMediator.CloudQueueClientTag)
                .InstancePerLifetimeScope();

            builder.Register(x => x.ResolveKeyed<CloudStorageAccount>(WebjobsStorage).CreateCloudBlobClient())
                .Keyed<CloudBlobClient>(WebjobsStorage)
                .Keyed<CloudBlobClient>(Acom.Search.Queue.Modules.SearchModule.SearchStorageTag)
                .InstancePerLifetimeScope()
                .OnActivated(x => x.Instance.DefaultRequestOptions.RetryPolicy = new ExponentialRetry());

            builder.Register(x => x.ResolveKeyed<CloudStorageAccount>(WebjobsStorageDefault).CreateCloudBlobClient())
               .Keyed<CloudBlobClient>(WebjobsStorageDefault)
               .Keyed<CloudBlobClient>(KnownVariables.CloudBlobClientTag)
               .InstancePerLifetimeScope()
               .OnActivated(x => x.Instance.DefaultRequestOptions.RetryPolicy = new ExponentialRetry());

            builder.RegisterGeneric(typeof(WebjobStorageRepository<>))
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
