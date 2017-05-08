namespace BusinessPlatform.Web.Infrastructure.Modules
{
    using Acom.Sitemap.Area;
    using Acom.Sitemap.Index;
    using Acom.Sitemap.Robots;
    using Autofac;
    using Features.Sitemap;

    public class SitemapModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RobotsGenerator>().AsSelf().SingleInstance();
            builder.RegisterType<SitemapCollection>().AsSelf().SingleInstance();
            builder.RegisterType<IndexGenerator>().AsSelf().SingleInstance();
            builder.RegisterType<AreaGenerator>().AsSelf().SingleInstance();
        }
    }
}