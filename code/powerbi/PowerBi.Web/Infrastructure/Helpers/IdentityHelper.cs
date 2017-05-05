using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace PowerBI.Web.Infrastructure.Helpers
{
    public static class IdentityHelper
    {
        public static string GetUsername(IIdentity userIdentity)
        {
            var username = userIdentity.ClaimValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(username))
            {
                username = userIdentity.ClaimValue("preferred_username");
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                username = userIdentity.Name;
            }

            return username;
        }

        public static string ClaimValue(this IIdentity userIdentity, string claimType)
        {
            var value = string.Empty;
            var claimsIdentity = userIdentity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == claimType);
                if (claim != null)
                {
                    value = claim.Value;
                }
            }

            return value;
        }
    }
}