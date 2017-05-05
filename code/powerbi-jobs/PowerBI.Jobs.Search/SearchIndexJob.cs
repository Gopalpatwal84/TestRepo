using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acom.Configuration;
using Acom.Configuration.Services;
using Acom.Search.Client;
using Acom.Search.Job.Host;
using Acom.Search.Job.Host.Handlers;
using Acom.Search.Queue.Internal;
using Autofac;
using Autofac.Features.OwnedInstances;
using Mediation.Webjobs;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Blob;
using PowerBI.Jobs.CommonModules;
using Serilog;

namespace PowerBI.Jobs.Search
{
    public class SearchIndexJob
    {
        private static IContainer container;
        private static bool regionHasSearchService;
        private static IndexBatcher batcher;
        private static IndexBatcher highPriorityBatcher;

        static void Main(string[] args)
        {
            container = InitializeContainer();

            regionHasSearchService = container.Resolve<ServiceSettings>().ServiceGroups.Default.SearchConnections.ForRegionOrNull(KnownAntaresVariables.RegionName) != null;

            container.Resolve<JobHostConfiguration>().Queues.BatchSize = IndexBatcher.BatchSize;
            IndexBatcher.IndexerFactory = () => container.Resolve<Owned<ISearchIndexer>>();
            batcher = new IndexBatcher();
            highPriorityBatcher = new IndexBatcher();

            container.Resolve<JobHost>().RunAndBlock();
        }

        public static async Task ProcessMessage([QueueTrigger("%IndexItemMessage%")] IndexItemMessage info,
                                                [Blob("%IndexItemMessage%/{BlobName}")] CloudBlockBlob blobInput,
                                                TextWriter log)
        {
            if (regionHasSearchService)
            {
                await container.HandleCommand(info, log, new TypedParameter(typeof(CloudBlockBlob), blobInput), new TypedParameter(typeof(IndexBatcher), batcher));
            }
        }

        public static async Task ProcessHighPriorityMessage([QueueTrigger("%IndexItemHighPriorityMessage%")] IndexItemHighPriorityMessage info,
                                                            [Blob("%IndexItemHighPriorityMessage%/{BlobName}")] CloudBlockBlob blobInput,
                                                            TextWriter log)
        {
            if (regionHasSearchService)
            {
                await container.HandleCommand<IndexItemMessage>(info, log, new TypedParameter(typeof(CloudBlockBlob), blobInput), new TypedParameter(typeof(IndexBatcher), highPriorityBatcher));
            }
        }

        [Singleton(KnownWebjobSettings.SlotNameField, Mode = SingletonMode.Listener)]
        public static Task UpdateIndexes([TimerTrigger(KnownTimerConfigs.AtMidnight, RunOnStartup = true)] TimerInfo timerInfo, TextWriter log)
        {
            if (regionHasSearchService)
            {
                try
                {
                    using (var scope = container.BeginScopeWithTextWriter(log))
                    {
                        var indexer = scope.Resolve<SearchIndexer>();
                        indexer.CreateOrUpdateEntriesIndex();
                        indexer.RemoveIndexes(scope.Resolve<AzureSearchConfiguration>().ObsoleteIndexes);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed update index.");
                    throw;
                }
            }
            return Task.FromResult(true);
        }

        private static IContainer InitializeContainer()
        {
            var builder = new ContainerBuilder();
            builder.InstallCommonPowerBiModules();
            builder.RegisterSearchHostModules();
            return builder.Build();
        }

    }
}
