using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Onyx.Contracts.Partners;
using PowerBI.Entities;

namespace PowerBI.Jobs.PartnersDirectoryPump.Features
{
    public static class PartnerExtensions
    {
        public static PartnerProfile DefaultProfile(this Partner partner)
        {
            return partner.Profiles.FirstOrDefault(p => p.Culture.Equals("en-us", StringComparison.InvariantCultureIgnoreCase)) ??
                   partner.Profiles.FirstOrDefault(p => p.Culture.StartsWith("en-", StringComparison.InvariantCultureIgnoreCase)) ??
                   partner.Profiles.First();
        }

        public static PartnerProfileDetailsExtendedContract DefaultProfile(this IEnumerable<PartnerProfileDetailsExtendedContract> profiles)
        {
            return profiles.FirstOrDefault(p => p.HasDefaultCulture()) ?? profiles.First();
        }

        public static string[] CountryCodes(this PartnerProfileDetailsExtendedContract profile)
        {
            return profile.ExtensionProperty("Countries").ToStringArray();
        }

        public static object ExtensionProperty(this PartnerProfileDetailsExtendedContract profile, string property)
        {
            return profile.ExtensionProperties.ContainsKey(property) ? profile.ExtensionProperties[property] : null;
        }

        public static bool HasDefaultCulture(this PartnerProfileDetailsExtendedContract profile)
        {
            var profileCulture = profile.ExtensionProperty("Culture") as string ?? string.Empty;

            return profileCulture.Equals("en-us", StringComparison.InvariantCultureIgnoreCase)
                || profileCulture.StartsWith("en-", StringComparison.InvariantCultureIgnoreCase);
        }

        private static string[] ToStringArray(this object obj)
        {
            var array = obj as JArray;
            if (array != null)
            {
                return array.Values<string>().ToArray();
            }

            return new string[] { };
        }
    }
}
