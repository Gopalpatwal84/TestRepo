using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Acom.IO;
using DevTrends.MvcDonutCaching;
using PowerBI.Entities;
using PowerBI.Web.Models.Pages.Integrations;
using PowerBI.Web.Models.Parts;

namespace PowerBI.Web.Controllers
{
    [DonutOutputCache(CacheProfile = "SiteCache")]
    public class IntegrationsController : BaseController
    {
        private readonly IRepository<Integration> integrationRepository;

        public IntegrationsController(IRepository<Integration> integrationRepository)
        {
            this.integrationRepository = integrationRepository;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/integrations/{slug}/")]
        public async Task<ActionResult> Details(string slug)
        {
            var model = new IntegrationDetails
            {
                Integration = await this.integrationRepository.GetAsync(slug),
            };

            if (model.Integration == null)
            {
                return this.NotFound();
            }

            model.IntegrationsList = await this.integrationRepository.GetAsync();
            
            return Request.QueryString["b"] == "05.30" ? 
                this.View("integrations/_details-b", model) :
                this.View("integrations/_details", model);
        }

        [ChildActionOnly]
        [Route("Integrations/IntegrationsSection")]
        public async Task<ActionResult> IntegrationsSection(string tag = "featured")
        {
            var integrations = await this.integrationRepository.GetAsync();

            var viewModel = new IntegrationsSectionViewModel
            {
                AllIntegrations = integrations,
                FeaturedIntegrations = integrations.Where(i => i.Tags?.Contains(tag) == true),
            };

            return PartialView("section/_integrations", viewModel);
        }
    }
}