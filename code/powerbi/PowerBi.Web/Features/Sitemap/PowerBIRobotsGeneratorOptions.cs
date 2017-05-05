using System.Linq;
using Acom.IO;
using Acom.Sitemap.Robots;
using PowerBI.Entities;
using PowerBI.Web.Configuration;

namespace PowerBI.Web.Features.Sitemap
{
    public class PowerBIRobotsGeneratorOptions : RobotsGeneratorOptions
    {
        public PowerBIRobotsGeneratorOptions(
            PowerBIConfiguration powerBiConfiguration,
            IRepository<Culture> culturesRepository)
        {
            var cultures = culturesRepository.Get();

            this.AllowedCultures = cultures.Where(y => y.IsDisplayed).Select(y => y.Slug).ToArray();
            this.ProductionDomain = powerBiConfiguration.ProductionDomain;
            this.DisallowedPaths = new[]
                {
                    "/*/search/?q=*",
                    "/patterns/"
                };
        }
    }
}