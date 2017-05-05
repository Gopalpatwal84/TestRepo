namespace PowerBI.Web.Infrastructure.Modules
{
    using System.Linq;
    using Acom.IO;
    using Acom.Sitemap.Area;
    using Acom.Sitemap.Index;
    using Acom.Sitemap.Robots;
    using Autofac;
    using Configuration;
    using Entities;
    using Features.Sitemap;
    using PowerBI.Web.Infrastructure.Helpers;

    public class SitemapModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SitemapCollection>().AsSelf().SingleInstance();
            builder.RegisterType<RobotsGenerator>().AsSelf().SingleInstance();
            builder.RegisterType<IndexGenerator>().AsSelf().SingleInstance();
            builder.RegisterType<AreaGenerator>().AsSelf().SingleInstance();
            builder.RegisterType<PowerBIRobotsGeneratorOptions>().As<RobotsGeneratorOptions>().SingleInstance();
        }
    }
}