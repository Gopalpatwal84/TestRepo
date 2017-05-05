using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Acom.IO;
using Acom.Search.Client;
using Acom.Search.Client.Models;
using DevTrends.MvcDonutCaching;
using Onyx.Client;
using Onyx.Client.Extensions.Partners;
using Onyx.Contracts.Partners;
using PowerBI.Entities;
using PowerBI.Search;
using PowerBI.Search.Documents;
using PowerBI.Web.Features.Partners;
using PowerBI.Web.Infrastructure.Helpers;
using PowerBI.Web.Models;
using PowerBI.Web.Models.Pages;
using PowerBI.Web.Models.Pages.Partners;

namespace PowerBI.Web.Controllers
{
    public class PartnersController : BaseController
    {
        protected const int s_pageSize = 24;

        private readonly IRepository<Partner> _partnersRepository;
        private readonly IRepository<Country> _countriesRepository;
        private readonly ISearchClient _searchClient;
        private readonly IOnyxRequestClient _onyxClient;

        public PartnersController(
            IRepository<Partner> partnersRepository,
            IRepository<Country> countriesRepository,
            ISearchClient searchClient,
            IOnyxRequestClient onyxClient)
        {
            _partnersRepository = partnersRepository;
            _countriesRepository = countriesRepository;
            _searchClient = searchClient;
            _onyxClient = onyxClient;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/partners/")]
        public async Task<ActionResult> Index(string country, int page = 1)
        {
            var facetSearchResponse = await _searchClient.CreateQuery<PartnerDirectoryProfileEntry>()
                .AddFacetField(c => c.Countries, 200)
                .Top(0)
                .IncludeTotalResultCount()
                .SearchAsync();

            var resolvedCountries = facetSearchResponse.Facets["countries"].Resolve(await _countriesRepository.GetAsync());

            if((this.Request.QueryString["country"] == null) && this.HttpContext.IsCloudPreference(KnownClouds.China))
            {
                country = "China";
            }

            var partnersSearchResults = await GetSearchResults(country, page, s_pageSize);
            var featuredPartnersResults = await GetFeauredPartners();

            var model = new PartnersModel
            {
                Pagination = new Pagination(page, s_pageSize, partnersSearchResults.Count),
                Partners = partnersSearchResults.Results.Select(x => new PartnerSearchViewModel { Entry = x.Document }).ToArray(),
                FeaturedPartners = featuredPartnersResults.Results.Select(x => new PartnerSearchViewModel { Entry = x.Document }).ToArray(),
                Countries = resolvedCountries.OrderBy(c => c.Slug).ToArray(),
                SelectedCountry = string.IsNullOrWhiteSpace(country) ? string.Empty : country
            };

            return this.View("partners/index", model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/partners/get-listed/")]
        public ActionResult GetListed()
        {
            return View("partners/get-listed", new ViewMetadataModel());
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/partners/{slug}/")]
        public async Task<ActionResult> Details(string slug)
        {
            var partner = await _partnersRepository.GetAsync(slug);
            if (partner == null)
            {
                return NotFound();
            }

            var featuredPartners = await this.GetFeauredPartners();

            var model = new PartnerDetailsModel
            {
                Partner = partner,
                FeaturedPartners = featuredPartners.Results.Select(x => new PartnerSearchViewModel { Entry = x.Document }).ToArray(),
            };

            return View("partners/_details", model);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [DonutOutputCache(NoStore = true, Duration = 0)]
        [Route("{culture=en-us}/partners/preview/{slug}/")]
        public async Task<ActionResult> DetailsPreview(string culture, string slug)
        {
            PartnerProfileContract profile = null;
            try
            {
                var slugParts = slug.ToLowerInvariant().Split(new string[] { "-powerbi-directory-" }, StringSplitOptions.None);

                if (slugParts.Length != 2)
                    return NotFound();

                profile = await _onyxClient.GetPartnerProfileAsync(slugParts[0], "powerbi-directory", slugParts[1]);
            }
            catch (HttpRequestException ex)
            {
                // We cannot get the status code from the exception so we use the exception message instead
                if (ex.Message.Contains("404 (Not Found)"))
                    return NotFound();
            }

            if (profile?.InReview == null)
                return NotFound();

            var featuredPartners = await this.GetFeauredPartners();

            var profileEntity = MapPartner(profile);
            var viewModel = new PartnerDetailsModel
            {
                Partner = new Partner { Profiles = new List<PartnerProfile> { profileEntity } },
                InPreview = true,
                FeaturedPartners = featuredPartners.Results.Select(x => new PartnerSearchViewModel { Entry = x.Document }).ToArray()
            };

            return View("partners/_details", viewModel);
        }

        private PartnerProfile MapPartner(PartnerProfileContract contract)
        {
            var profile = contract.InReview;

            return new PartnerProfile
            {
                Id = contract.PartnerId,
                Culture = profile.Culture(),
                Countries = profile.CountryCodes()
                                .Select(code => _countriesRepository.GetAsync().Result.FirstOrDefault(x => x.Code == code))
                                .Where(x => x != null)
                                .ToArray(),
                LogoUrl = profile.Logo,
                Name = profile.Name,
                Website = profile.Website,
                ContactEmail = profile.ContactEmail,
                ContactPhone = profile.ContactPhone,
                PartnerId = contract.PartnerId,
                Description = profile.ExtensionProperty("ServiceDescription") as string,
            };
        }

        private async Task<AcomDocumentSearchResults<PartnerDirectoryProfileEntry>> GetFeauredPartners()
        {
            return await _searchClient.CreateQuery<PartnerDirectoryProfileEntry>()
                .OrderByProperty(p => p.TitleCanonical)
                .FilterByProperty(x => x.Featured, true)
                .SearchAsync();
        }

        private async Task<AcomDocumentSearchResults<PartnerDirectoryProfileEntry>> GetSearchResults(string country, int page, int pageSize)
        {
            return await _searchClient.CreateQuery<PartnerDirectoryProfileEntry>()
                .IncludeTotalResultCount()
                .OrderByProperty(p => p.TitleCanonical)
                .AnyTermInCollection(x => x.Countries, country)
                .Skip(pageSize * (page - 1))
                .Top(pageSize)
                .SearchAsync();
        }
    }
}