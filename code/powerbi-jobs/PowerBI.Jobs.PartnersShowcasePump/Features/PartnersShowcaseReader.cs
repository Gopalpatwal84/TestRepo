using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acom.IO.Json;
using Onyx.Client;
using Onyx.Client.Extensions.Partners;
using Onyx.Contracts;
using Onyx.Contracts.Partners;
using PowerBI.Entities;
using PowerBI.Entities.Partners;

namespace PowerBI.Jobs.PartnersShowcasePump.Features
{
    public class PartnersShowcaseReader
    {
        private readonly IOnyxRequestClient _onyxClient;
        private readonly JsonLoader<Country> _countriesLoader;
        private readonly JsonLoader<ShowcaseIndustry> _industriesLoader;
        private readonly JsonLoader<ShowcaseDepartment> _departmentsLoader;
        private readonly JsonLoader<PartnerCompetency> _competenciesLoader;

        public PartnersShowcaseReader(
            IOnyxRequestClient onyxClient,
            JsonLoader<Country> countriesLoader,
            JsonLoader<ShowcaseIndustry> industriesLoader,
            JsonLoader<ShowcaseDepartment> departmentsLoader,
            JsonLoader<PartnerCompetency> competenciesLoader)
        {
            _onyxClient = onyxClient;
            _countriesLoader = countriesLoader;
            _industriesLoader = industriesLoader;
            _departmentsLoader = departmentsLoader;
            _competenciesLoader = competenciesLoader;
        }

        public async Task<PartnerShowcase[]> Fetch()
        {
            var allProfiles = (await _onyxClient.GetApprovedPartnerProfilesAsync("powerbi", "&limit=1000", 1000)).PartnerProfiles;

            var partners = new List<PartnerShowcase>();
            var partnerGroups = allProfiles.GroupBy(profile => profile.PartnerId);

            foreach (var partner in partnerGroups)
            {
                var partnerSolutions = partner.GroupBy(profile => profile.ExtensionProperty("SolutionSlug") as string)
                    .Where(solutionGroup => !string.IsNullOrWhiteSpace(solutionGroup.Key))
                    .Select(solutionGroup => new PartnerShowcase
                    {
                        Slug = ResolvePartnerSlug(partner.Key, solutionGroup),
                        Profiles = MapProfiles(solutionGroup)
                    })
                    .Where(x => x.Profiles.Any());

                partners.AddRange(partnerSolutions);
            }

            return partners.ToArray();
        }

        private string ResolvePartnerSlug(string partnerId, IGrouping<string, PartnerProfileDetailsExtendedContract> profiles)
        {
            var webSlug = profiles.Select(profile => profile.ExtensionProperty("WebSlug") as string)
                            .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

            if (!string.IsNullOrWhiteSpace(webSlug))
            {
                return webSlug;
            }

            return $"{partnerId}-{profiles.Key}";
        }

        private IEnumerable<PartnerShowcaseProfile> MapProfiles(IGrouping<string, PartnerProfileDetailsExtendedContract> profiles)
        {
            var allCountries = _countriesLoader.Data.Value;

            return profiles.Select(profile => new PartnerShowcaseProfile
            {
                Culture = profile.ExtensionProperty("Culture") as string,
                SolutionName = profile.ExtensionProperty("SolutionName") as string,
                Competencies = _competenciesLoader.Data.Value.ResolveFromSlugs(profile.CompetencySlugs()),
                Industries = _industriesLoader.Data.Value.ResolveFromSlugs(profile.IndustrySlugs()),
                Departments = _departmentsLoader.Data.Value.ResolveFromSlugs(profile.DepartmentSlugs()),
                Countries = profile.CountryCodes()
                                .Select(code => allCountries.FirstOrDefault(x => x.Code == code))
                                .Where(x => x != null)
                                .ToArray(),
                ShortDescription = profile.ExtensionProperty("ShortDesc") as string,
                LongDescription = profile.ExtensionProperty("LongDesc") as string,
                SolutionDescriptionVideo = profile.ExtensionProperty("DescVideo") as string,
                CaseStudyVideo = profile.ExtensionProperty("CaseStudyVideo") as string,
                ReportId = profile.ExtensionProperty("ReportId") as string,
                PinpointUrl = profile.ExtensionProperty("PinpointUrl") as string,
                ContactUrl = profile.ExtensionProperty("ContactUrl") as string,
                Screenshots = profile.Screenshots(),
                LogoUrl = profile.Logo,
                ThumbnailUrl = profile.Thumbnail,
                Name = profile.Name,
                Website = profile.Website,
                ContactEmail = profile.ContactEmail,
                ContactPhone = profile.ContactPhone,
                PartnerId = profile.PartnerId,
                Id = profile.ProfileId
            })
            .Where(x => !string.IsNullOrEmpty(x.ShortDescription))
            .ToArray();
        }
    }
}
