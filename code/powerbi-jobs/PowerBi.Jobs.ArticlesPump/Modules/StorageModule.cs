using System.Configuration;
using Autofac;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace PowerBI.Jobs.ArticlesPump.Modules
{
    public class StorageModule : Module
    {
        public static object ArticlesStorage = new object();

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => CloudStorageAccount.Parse(ConfigurationManager.AppSettings["ArticlesConnectionString"]))
                .Keyed<CloudStorageAccount>(ArticlesStorage)
                .SingleInstance();

            builder.Register(x => x.ResolveKeyed<CloudStorageAccount>(ArticlesStorage).CreateCloudBlobClient())
                .Keyed<CloudBlobClient>(ArticlesStorage)
                .InstancePerLifetimeScope()
                .OnActivated(x => x.Instance.DefaultRequestOptions.RetryPolicy = new ExponentialRetry());

            builder.Register(x => x.ResolveKeyed<CloudBlobClient>(ArticlesStorage).GetContainerReference(ConfigurationManager.AppSettings["ArticlesContainer"]))
                .Keyed<CloudBlobContainer>(ArticlesStorage)
                .InstancePerLifetimeScope();
        }
    }
}
