using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Hosting;
using Microsoft.Owin.Security.Notifications;
using Newtonsoft.Json;
using PowerBI.Web.Infrastructure.Helpers;
using Serilog;

namespace PowerBI.Web.Authorization
{
    public class TenantAuthorizationAgent : IAuthorizationAgent
    {
        private static IEnumerable<Tenant> Tenants = null;

        public async Task<AuthorizeResult> AuthorizeAsync(AuthorizationCodeReceivedNotification context)
        {
            var identity = context.AuthenticationTicket.Identity;
            var email = IdentityHelper.GetUsername(identity);
            var tenantClaim = identity.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid");
            if (tenantClaim == null)
            {
                Log.Error("Did not find tenant claim (http://schemas.microsoft.com/identity/claims/tenantid) for user {user} ", email);
                return AuthorizeResult.DenyConditional;
            }

            var tenantId = tenantClaim.Value;

            if (IsAllowedTenant(tenantId))
            {
                Log.Information("Tenant {tenant} (from user {user}) is listed as pre-allowed", tenantId, email);

                identity.AddClaim(new Claim(ClaimTypes.Role, KnownRoles.PowerBIUser));

                Log.Information("Role added for user {user}: {role}", email, KnownRoles.PowerBIUser);

                return AuthorizeResult.Allow;
            }
            else
            {
                Log.Information("Tenant {tenant} (from user {user}) is not listed as pre-allowed", tenantId, email);

                if (FilterConfig.LockdownEnabled())
                {
                    return AuthorizeResult.Deny;
                }

                return AuthorizeResult.DenyConditional;
            }
        }

        public bool IsAllowedTenant(string tenantId)
        {
            if (Tenants == null)
            {
                // Tenants from tenants.json
                var tenantsList = File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/tenants.json"));
                Tenants = JsonConvert.DeserializeObject<List<Tenant>>(tenantsList);
            }

            return Tenants.Any(t => tenantId.Equals(t.TenantID, StringComparison.OrdinalIgnoreCase));
        }

        private class Tenant
        {
            public string TenantID { get; set; }

            public string Organization { get; set; }

            public string Comments { get; set; }
        }
    }
}