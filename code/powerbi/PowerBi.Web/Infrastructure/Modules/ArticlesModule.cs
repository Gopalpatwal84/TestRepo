using Acom.Articles;
using Acom.IO;
using Acom.Redis;
using Autofac;

namespace PowerBI.Web.Infrastructure.Modules
{
    public class ArticlesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RedisRepository<ArticleContent>>().As<IRepository<ArticleContent>>().SingleInstance();
        }
    }
}