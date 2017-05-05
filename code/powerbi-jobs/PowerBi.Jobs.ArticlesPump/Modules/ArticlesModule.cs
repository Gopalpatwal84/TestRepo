using System.Linq;
using Acom.Articles;
using Acom.IO.Json;
using Autofac;
using Microsoft.WindowsAzure.Storage.Blob;
using PowerBI.Entities;
using PowerBI.Jobs.ArticlesPump.Features;

namespace PowerBI.Jobs.ArticlesPump.Modules
{
    public class ArticlesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PowerBiArticleParser>()
                .As<ArticleParser<Article>>()
                .InstancePerLifetimeScope();

            builder.Register(x =>
                new ArticlesReader<Article>(x.ResolveKeyed<CloudBlobContainer>(StorageModule.ArticlesStorage),
                    x.Resolve<ArticleParser<Article>>(),
                    x.Resolve<JsonLoader<Culture>>()
                        .Data.Value
                        .Select(culture => culture.Slug).ToArray()));
        }
    }
}
