namespace BusinessPlatform.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Acom.Configuration;
    using Acom.IO;
    using Acom.Sitemap.Area;
    using Acom.Sitemap.Index;
    using Acom.Sitemap.Robots;
    using BusinessPlatform.Entities;
    using BusinessPlatform.Web.Configuration;
    using Features.Sitemap;

    public class SitemapController : Controller
    {
        private readonly RobotsGenerator robotsGenerator;
        private readonly BusinessPlatformConfiguration businessPlatformConfiguration;
        private readonly IRepository<Culture> culturesRepository;
        private readonly SitemapCollection sitemapCollection;
        private readonly IndexGenerator indexGenerator;
        private readonly AreaGenerator areaGenerator;

        public SitemapController(
            IRepository<Culture> culturesRepository,
            RobotsGenerator robotsGenerator,
            BusinessPlatformConfiguration businessPlatformConfiguration,
            SitemapCollection sitemapCollection,
            IndexGenerator indexGenerator,
            AreaGenerator areaGenerator)
        {
            this.culturesRepository = culturesRepository;
            this.robotsGenerator = robotsGenerator;
            this.businessPlatformConfiguration = businessPlatformConfiguration;
            this.sitemapCollection = sitemapCollection;
            this.indexGenerator = indexGenerator;
            this.areaGenerator = areaGenerator;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("robots.txt")]
        public async Task<ActionResult> Robots()
        {
            RobotsViewModel viewModel;
            if (KnownSlots.IsProduction)
            {
                var cultures = await this.culturesRepository.GetAsync();
                var options = new RobotsGeneratorOptions
                {
                    AllowedCultures = cultures.Where(x => x.IsDisplayed).Select(x => x.Slug).ToArray(),
                    ProductionDomain = this.businessPlatformConfiguration.ProductionDomain,
                };

                viewModel = this.robotsGenerator.GenerateProductionModel(options);
            }
            else
            {
                viewModel = this.robotsGenerator.GeneratePreproductionModel();
            }

            return this.View("robots", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("robotsitemap/{culture}")]
        public async Task<ActionResult> SitemapIndex(string culture)
        {
            var options = new IndexGeneratorOptions
            {
                Culture = culture,
                Domain = this.businessPlatformConfiguration.ProductionDomain,
                Pages = await this.sitemapCollection.GetSitemapPageInfosAsync(HttpContext.Cache),
            };

            return this.Content(this.indexGenerator.GenerateIndexXml(options), "text/xml");
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("robotsitemap/{culture}/{*area}")]
        public async Task<ActionResult> SitemapArea(string culture, string area)
        {
            var cultures = await this.culturesRepository.GetAsync();
            var options = new AreaGeneratorOptions
            {
                CurrentCulture = culture,
                Domain = this.businessPlatformConfiguration.ProductionDomain,
                AllCultures = cultures.Where(x => x.IsDisplayed).Select(x => x.Slug).ToArray(),
                AreaName = area,
                Pages = await this.sitemapCollection.GetSitemapPageInfosAsync(HttpContext.Cache),
            };

            return this.Content(this.areaGenerator.GenerateAreaXml(options), "text/xml");
        }
    }
}