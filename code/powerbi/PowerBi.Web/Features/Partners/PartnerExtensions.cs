using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Onyx.Contracts.Partners;

namespace PowerBI.Web.Features.Partners
{
    public static class PartnerExtensions
    {
        public static string Culture(this PartnerProfileDetailsContract profile)
        {
            return profile.ExtensionProperty("Culture") as string ?? string.Empty;
        }

        public static string[] IndustrySlugs(this PartnerProfileDetailsContract profile)
        {
            return profile.ExtensionProperty("Industries").ToStringArray();
        }

        public static string[] DepartmentSlugs(this PartnerProfileDetailsContract profile)
        {
            return profile.ExtensionProperty("BusinessDepartments").ToStringArray();
        }

        public static string[] CountryCodes(this PartnerProfileDetailsContract profile)
        {
            return profile.ExtensionProperty("Countries").ToStringArray();
        }

        public static string[] CompetencySlugs(this PartnerProfileDetailsContract profile)
        {
            return profile.ExtensionProperty("Competencies").ToStringArray();
        }

        public static object ExtensionProperty(this PartnerProfileDetailsContract profile, string property)
        {
            return profile.ExtensionProperties.ContainsKey(property) ? profile.ExtensionProperties[property] : null;
        }

        public static IEnumerable<string> Screenshots(this PartnerProfileDetailsContract profile)
        {
            var screenshots = new List<string>();

            for (int i = 1; i <= 3; i++)
            {
                var screenshotPath = profile.ExtensionProperty(string.Format($"PowerbiShowcasePbixScreenshot{i}FullPath")) as string;
                if (!string.IsNullOrWhiteSpace(screenshotPath))
                {
                    screenshots.Add(screenshotPath);
                }
            }

            return screenshots;
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