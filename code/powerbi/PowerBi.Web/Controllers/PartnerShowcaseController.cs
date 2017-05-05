using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Acom.IO;
using Acom.Search.Client;
using DevTrends.MvcDonutCaching;
using Onyx.Client;
using Onyx.Client.Extensions.Partners;
using Onyx.Contracts.Partners;
using PowerBI.Entities;
using PowerBI.Entities.Partners;
using PowerBI.Search;
using PowerBI.Search.Documents;
using PowerBI.Web.Infrastructure.Helpers;
using PowerBI.Web.Models.Pages.PartnerShowcases;
using System;

namespace PowerBI.Web.Controllers
{
    public class PartnerShowcaseController : BaseController
    {
        private readonly IRepository<PartnerShowcase> _partnerShowcaseRepository;
        private readonly IRepository<ShowcaseIndustry> _industriesRepository;
        private readonly IRepository<ShowcaseDepartment> _departmentsRepository;
        private readonly IRepository<Country> _countriesRepository;
        private readonly IRepository<PartnerCompetency> _competenciesRepository;
        private readonly ISearchClient _searchClient;
        private readonly IOnyxRequestClient _onyxClient;

        public PartnerShowcaseController(
            IRepository<PartnerShowcase> partnerShowcaseRepository, 
            IRepository<ShowcaseIndustry> industriesRepository,
            IRepository<ShowcaseDepartment> departmentsRepository,
            IRepository<Country> countriesRepository,
            ISearchClient searchClient,
            IRepository<PartnerCompetency> competenciesRepository,
            IOnyxRequestClient onyxClient)
        {
            _partnerShowcaseRepository = partnerShowcaseRepository;
            _industriesRepository = industriesRepository;
            _departmentsRepository = departmentsRepository;
            _countriesRepository = countriesRepository;
            _searchClient = searchClient;
            _competenciesRepository = competenciesRepository;
            _onyxClient = onyxClient;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/partner-showcase/")]
        public async Task<ActionResult> Index(PartnerShowcaseSearchOptions searchOptions)
        {
            var facetSearchResponse = await _searchClient.CreateQuery<PartnerShowcaseProfileEntry>()
                .AddFacetField(c => c.Industries, 100)
                .AddFacetField(c => c.Departments, 100)
                .AddFacetField(c => c.Countries, 200)
                .Top(0)
                .IncludeTotalResultCount()
                .SearchAsync();
            
            var resolvedIndustries = facetSearchResponse.Facets["industries"].Resolve(await _industriesRepository.GetAsync());
            var resolvedDepartments = facetSearchResponse.Facets["departments"].Resolve(await _departmentsRepository.GetAsync());
            var resolvedCountries = facetSearchResponse.Facets["countries"].Resolve(await _countriesRepository.GetAsync());

            if ((this.Request.QueryString["country"] == null) && this.HttpContext.IsCloudPreference(KnownClouds.China)) 
            {
                searchOptions.Country = "China";
            }

            var partnersSearchResults = await GetSearchResults(searchOptions);
            var featuredPartnersResults = await GetFeaturedResults();

            var model = new PartnerShowcaseViewModel
            {
                Partners = partnersSearchResults,
                FeaturedPartners = featuredPartnersResults,
                Countries = resolvedCountries,
                Industries = resolvedIndustries,
                Departments = resolvedDepartments,
                SelectedCountry = string.IsNullOrWhiteSpace(searchOptions.Country) ? string.Empty : searchOptions.Country,
                SelectedIndustry = string.IsNullOrWhiteSpace(searchOptions.Industry) ? string.Empty : searchOptions.Industry,
                SelectedDepartment = string.IsNullOrWhiteSpace(searchOptions.Department) ? string.Empty : searchOptions.Department,
                SearchTerm = searchOptions.Term
            };

            return View("partner-showcase/index", model);
        }

        private async Task<IEnumerable<PartnerShowcaseSearchViewModel>> GetSearchResults(PartnerShowcaseSearchOptions searchOptions)
        {
            var searchResponse = await _searchClient.CreateQuery<PartnerShowcaseProfileEntry>(searchOptions.Term)
                .OrderByProperty(p => p.TitleCanonical)
                .AnyTermInCollection(x => x.Industries, searchOptions.Industry)
                .AnyTermInCollection(x => x.Departments, searchOptions.Department)
                .AnyTermInCollection(x => x.Countries, searchOptions.Country)
                .SearchAllAsync();

            return searchResponse.Select(x => new PartnerShowcaseSearchViewModel { Entry = x });
        }

        private async Task<IEnumerable<PartnerShowcaseSearchViewModel>> GetFeaturedResults()
        {
            var searchResponse = await _searchClient.CreateQuery<PartnerShowcaseProfileEntry>()
                .OrderByProperty(p => p.TitleCanonical)
                .FilterByProperty(x => x.Featured, true)
                .SearchAllAsync();

            return searchResponse.Select(x => new PartnerShowcaseSearchViewModel { Entry = x });
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/partner-showcase/{slug}/")]
        public async Task<ActionResult> Details(string slug)
        {
            var partner = await _partnerShowcaseRepository.GetAsync(slug);
            if (partner == null)
            {
                return NotFound();
            }

            var model = new PartnerShowcaseDetailsViewModel
            {
                Partner = partner
            };

            return View("partner-showcase/_details", model);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [DonutOutputCache(NoStore = true, Duration = 0)]
        [Route("{culture=en-us}/partner-showcase/preview/{slug}/")]
        public async Task<ActionResult> DetailsPreview(string culture, string slug)
        {
            PartnerProfileContract profile = null;
            try
            {
                var slugParts = slug.ToLowerInvariant().Split(new string[] { "-powerbi-" }, StringSplitOptions.None);

                if (slugParts.Length != 2)
                    return NotFound();

                profile = await _onyxClient.GetPartnerProfileAsync(slugParts[0], "powerbi", slugParts[1]);
            }
            catch (HttpRequestException ex)
            {
                // We cannot get the status code from the exception so we use the exception message instead
                if (ex.Message.Contains("404 (Not Found)"))
                    return NotFound();
            }

            if (profile?.InReview == null)
                return NotFound();

            var profileEntity = await this.MapPartner(profile);
            var viewModel = new PartnerShowcaseDetailsViewModel
            {
                Partner = new PartnerShowcase { Profiles = new List<PartnerShowcaseProfile> { profileEntity } },
                InPreview = true
            };

            return View("partner-showcase/_details", viewModel);
        }

        private async Task<PartnerShowcaseProfile> MapPartner(PartnerProfileContract contract)
        {
            var profile = contract.InReview;

            return new PartnerShowcaseProfile
            {
                Id = contract.PartnerId,
                Culture = profile.Culture(),
                Competencies = (await _competenciesRepository.GetAsync()).Where(x => profile.CompetencySlugs().Contains(x.Slug)),
                Industries = (await _industriesRepository.GetAsync()).Where(x => profile.IndustrySlugs().ToList().Contains(x.Slug)),
                Departments = (await _departmentsRepository.GetAsync()).Where(x => profile.DepartmentSlugs().ToList().Contains(x.Slug)),
                Countries = profile.CountryCodes()
                                .Select(code => _countriesRepository.GetAsync().Result.FirstOrDefault(x => x.Code == code))
                                .Where(x => x != null)
                                .ToArray(),
                ShortDescription = profile.ExtensionProperty("ShortDesc") as string,
                LongDescription = profile.ExtensionProperty("LongDesc") as string,
                SolutionDescriptionVideo = profile.ExtensionProperty("DescVideo") as string,
                CaseStudyVideo = profile.ExtensionProperty("CaseStudyVideo") as string,
                ReportId = profile.ExtensionProperty("ReportId") as string,
                PinpointUrl = profile.ExtensionProperty("PinpointUrl") as string,
                Screenshots = profile.Screenshots(),
                LogoUrl = profile.Logo,
                Name = profile.Name,
                Website = profile.Website,
                ContactEmail = profile.ContactEmail,
                ContactPhone = profile.ContactPhone,
                PartnerId = contract.PartnerId
            };
        }
        
        public class PartnerShowcaseSearchOptions
        {
            public string Term { get; set; }
            public string Industry { get; set; }
            public string Department { get; set; }
            public string Country { get; set; }
        }
    }
}
