namespace PowerBI.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Acom.IO;
    using Acom.IO.Json;
    using Acom.Search.Client.Documents;
    using Authorization;
    using DevTrends.MvcDonutCaching;
    using Infrastructure.Helpers;
    using Models.Pages.Search;
    using Newtonsoft.Json;
    using PowerBI.Entities;
    using PowerBI.Web.Models.Pages;
    using Search.Documents;
    using Serilog;

    [DonutOutputCache(CacheProfile = "CloudPreferenceCache")]
    public class SupportController : BaseController
    {
        private IRepository<TicketingDisabledDate> Repository { get; set; }
        private readonly SearchHelper searchHelper;

        private IJsonRequestHelper RequestHelper { get; set; }

        public SupportController(IRepository<TicketingDisabledDate> repository, IJsonRequestHelper requestHelper, SearchHelper searchHelper)
        {
            this.Repository = repository;
            this.RequestHelper = requestHelper;
            this.searchHelper = searchHelper;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/support/")]
        public async Task<ActionResult> Index(string Mode)
        {
            var viewModel = new SupportViewModel();

            var supportConfiguration = new Configuration.PowerBIConfiguration();
            var serviceStatusUrl = supportConfiguration.ServiceStatusUrl;
            if (Mode == "test")
            {
                serviceStatusUrl = supportConfiguration.TestServiceStatusUrl;
            }

            // Get service status
            try
            {
                var uri = new Uri(serviceStatusUrl);
                var json = await this.RequestHelper.IssueJsonRequest(uri);

                if (!string.IsNullOrEmpty(json))
                {
                    dynamic result = JsonConvert.DeserializeObject(json);
                    
                    viewModel.Status = result.article.formatted_text;
                }
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
            }

            var disabledDates = await this.Repository.GetAsync();
            var today = DateTime.Now;

            viewModel.TicketingDisabledMessage = disabledDates
                .FirstOrDefault(range => range.StartDate <= today && today <= range.EndDate)?
                .Message;

            return View("support/index", viewModel);
        }

        [Authorize(Roles = KnownRoles.SupportPro)]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/support/pro/")]
        public async Task<ActionResult> SupportPro(string q, string type)
        {
            var page = 1;
            var pageSize = 5;

            SearchIndexModel model;

            if (!string.IsNullOrEmpty(q))
            {
                switch (type)
                {
                    case "documentation":
                        model = await this.searchHelper.Search<ArticleEntry>(q, page, pageSize);
                        break;

                    case "blog":
                        model = await this.searchHelper.Search<BlogEntry>(q, page, pageSize);
                        break;

                    case "community":
                        model = await this.searchHelper.Search<CommunityEntry>(q, page, pageSize);
                        break;

                    case "partners":
                        model = await this.searchHelper.Search<PartnerDirectoryProfileEntry>(q, page, pageSize);
                        break;

                    default:
                        model = await this.searchHelper.Search<Entry>(q, page, pageSize);
                        break;
                }
            }
            else
            {
                model = new SearchIndexModel
                {
                    Types = SearchHelper.AvailableTypes,
                    Type = string.Empty
                };
            }

            return this.View("support/pro/index", model);
        }

        [Authorize(Roles = KnownRoles.SupportPro)]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/support/pro/ticket/")]
        public ActionResult SupportTicket(string q, string type)
        {
            var tenantId = User.Identity.ClaimValue(KnownClaimTypes.TenantId);
            if (tenantId.Equals(KnownTenants.Microsoft, StringComparison.OrdinalIgnoreCase))
            {
                return this.Redirect("https://microsoft.sharepoint.com/sites/biatmicrosoft/Pages/Support/Default.aspx#requestSupport");
            }

            return this.View("support/pro/ticket", new ViewMetadataModel());
        }
    }
}