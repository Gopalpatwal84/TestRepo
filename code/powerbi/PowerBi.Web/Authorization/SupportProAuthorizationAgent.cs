using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Acom.IO;
using Microsoft.Owin.Security.Notifications;
using PowerBI.Entities;
using PowerBI.Web.Infrastructure.Helpers;
using Serilog;

namespace PowerBI.Web.Authorization
{
    public class SupportProAuthorizationAgent : IAuthorizationAgent
    {
        private readonly IRepository<SupportPlan> supportPlansRepository;

        public SupportProAuthorizationAgent()
        {
            this.supportPlansRepository = DependencyResolver.Current.GetService<IRepository<SupportPlan>>();
        }

        public async Task<AuthorizeResult> AuthorizeAsync(AuthorizationCodeReceivedNotification context)
        {
            var clientId = ConfigurationManager.AppSettings["ActiveDirectoryClientID"];
            var clientSecret = ConfigurationManager.AppSettings["ActiveDirectoryPassword"];

            var identity = context.AuthenticationTicket.Identity;
            var email = IdentityHelper.GetUsername(identity);

            var redirectUri = new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority).ToString() + "/");
            var accessToken = await GraphApiHelper.GetAccessTokenAsync(context.Code, redirectUri);
            if (string.IsNullOrEmpty(accessToken))
            {
                Log.Warning("Not able to obtain an Access Token for user {user} ", email);
                return AuthorizeResult.DenyConditional;
            }
            else
            {
                Log.Information("Access Token obtained for user {user} ", email);
            }

            var userProfile = await GraphApiHelper.GetUserProfileAsync(accessToken);

            if (userProfile == null)
            {
                Log.Warning("Not able to obtain profile for user {user} ", email);
                return AuthorizeResult.DenyConditional;
            }

            var assignedPlans = new List<string>();
            if (userProfile.AssignedPlans != null)
            {
                assignedPlans.AddRange(userProfile.AssignedPlans.Select(l => l.ServicePlanId));
            }

            var supportPlans = await this.supportPlansRepository.GetAsync();
            
            if (supportPlans.Any(s => assignedPlans.Contains(s.Id, StringComparer.OrdinalIgnoreCase)))
            {
                AddSupportRequiredClaims(identity, userProfile, accessToken);

                Log.Information("Role added for user {user}: {role}", email, KnownRoles.SupportPro);
                return AuthorizeResult.AllowConditional;
            }
            else
            {
                Log.Information("User {user} does not have a Pro Support plan", email);
                return AuthorizeResult.DenyConditional;
            }
        }

        private void AddSupportRequiredClaims(ClaimsIdentity identity, UserProfile userProfile, string accessToken)
        {
            // Add claims required for Support ticket creation
            if (!string.IsNullOrWhiteSpace(userProfile.CompanyName))
            {
                identity.AddClaim(new Claim(KnownClaimTypes.CompanyName, userProfile.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(userProfile.ObjectId) &&
                !identity.Claims.Any(c => c.Type.Equals(KnownClaimTypes.ObjectIdentifier, StringComparison.OrdinalIgnoreCase)))
            {
                identity.AddClaim(new Claim(KnownClaimTypes.ObjectIdentifier, userProfile.ObjectId));
            }

            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(accessToken))
            {
                var token = handler.ReadToken(accessToken) as JwtSecurityToken;
                var claim = token.Claims.SingleOrDefault(c => c.Type.Equals("puid", StringComparison.OrdinalIgnoreCase));

                identity.AddClaim(new Claim(KnownClaimTypes.Puid, claim?.Value ?? string.Empty));
            }

            // Enable user for Support Pro
            identity.AddClaim(new Claim(ClaimTypes.Role, KnownRoles.SupportPro));
        }
    }
}