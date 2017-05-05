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

namespace PowerBI.Jobs.PartnersDirectoryPump.Features
{
    public class PartnersDirectoryReader
    {
        private readonly IOnyxRequestClient _onyxClient;
        private readonly JsonLoader<Country> _countriesLoader;

        public PartnersDirectoryReader(
            IOnyxRequestClient onyxClient,
            JsonLoader<Country> countriesLoader)
        {
            _onyxClient = onyxClient;
            _countriesLoader = countriesLoader;
        }

        public async Task<Partner[]> Fetch()
        {
            var allProfiles = (await _onyxClient.GetApprovedPartnerProfilesAsync("powerbi-directory", "&limit=1000", 1000)).PartnerProfiles;

            var partnerGroups = allProfiles.GroupBy(profile => profile.PartnerId);

            return partnerGroups.Select(partnerGroup => new Partner
            {
                Slug = partnerGroup.Key,
                Profiles = MapProfiles(partnerGroup),
            })
            .Where(x => x.Profiles.Any())
            .ToArray();
        }

        private IEnumerable<PartnerProfile> MapProfiles(IGrouping<string, PartnerProfileDetailsExtendedContract> profiles)
        {
            var allCountries = _countriesLoader.Data.Value;

            return profiles.Select(profile => new PartnerProfile
            {
                Culture = profile.ExtensionProperty("Culture") as string ?? string.Empty,
                Countries = profile.CountryCodes()
                                .Select(code => allCountries.FirstOrDefault(x => x.Code == code))
                                .Where(x => x != null)
                                .ToArray(),
                Description = profile.ExtensionProperty("ServiceDescription") as string ?? string.Empty,
                LogoUrl = profile.Logo,
                Name = profile.Name,
                Website = profile.Website,
                ContactEmail = profile.ContactEmail,
                ContactPhone = profile.ContactPhone,
                PartnerId = profile.PartnerId,
                Id = profile.ProfileId
            })
            .Where(x => !string.IsNullOrEmpty(x.Description))
            .ToArray();
        }
    }
}
