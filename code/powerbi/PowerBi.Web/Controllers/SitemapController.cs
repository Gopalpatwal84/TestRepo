using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Acom.Configuration;
using Acom.IO;
using Acom.Sitemap.Area;
using Acom.Sitemap.Index;
using Acom.Sitemap.Robots;
using PowerBI.Entities;
using PowerBI.Web.Configuration;
using PowerBI.Web.Infrastructure.Helpers;

namespace PowerBI.Web.Controllers
{
    public class SitemapController : Controller
    {
        private readonly AreaGenerator _areaGenerator;
        private readonly IndexGenerator _indexGenerator;
        private readonly PowerBIConfiguration _powerBiConfiguration;
        private readonly RobotsGenerator _robotsGenerator;
        private readonly IRepository<Culture> _culturesRepository;
        private readonly SitemapCollection _sitemapCollection;

        public SitemapController(
            IndexGenerator indexGenerator,
            AreaGenerator areaGenerator,
            SitemapCollection sitemapCollection,
            RobotsGenerator robotsGenerator,
            IRepository<Culture> culturesRepository,
            PowerBIConfiguration powerBIConfiguration)
        {
            _indexGenerator = indexGenerator;
            _areaGenerator = areaGenerator;
            _sitemapCollection = sitemapCollection;
            _robotsGenerator = robotsGenerator;
            _culturesRepository = culturesRepository;
            _powerBiConfiguration = powerBIConfiguration;
        }

        [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("robotsitemap/{culture}/")]
        public async Task<ContentResult> SitemapIndex(string culture)
        {
            var options = new IndexGeneratorOptions
            {
                Culture = culture,
                Domain = _powerBiConfiguration.ProductionDomain,
                Pages = await _sitemapCollection.GetSitemapPageInfosAsync(HttpContext.Cache)
            };

            return Content(_indexGenerator.GenerateIndexXml(options), "text/xml");
        }

        [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("robotsitemap/{culture}/{*area}")]
        public async Task<ContentResult> Sitemap(string culture, string area)
        {
            var cultures = await _culturesRepository.GetAsync();
            var options = new AreaGeneratorOptions
            {
                CurrentCulture = culture,
                Domain = _powerBiConfiguration.ProductionDomain,
                AllCultures = cultures.Where(x => x.IsDisplayed).Select(x => x.Slug).ToArray(),
                AreaName = area,
                Pages = await _sitemapCollection.GetSitemapPageInfosAsync(HttpContext.Cache)
            };

            return Content(_areaGenerator.GenerateAreaXml(options), "text/xml");
        }
    }
}