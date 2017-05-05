using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.ActiveDirectory;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using PowerBI.Web.Authorization;
using PowerBI.Web.Infrastructure.Helpers;
using Serilog;

namespace PowerBI.Web
{
    public partial class Startup
    {
        private Uri originalRequestUri;

        private static Dictionary<string, string> redirectMapping = new Dictionary<string, string>
        {
            { "/support/pro", "/support/free/" }
        };

        public void ConfigureAuth(IAppBuilder app)
        {
            var clientId = ConfigurationManager.AppSettings["ActiveDirectoryClientID"];
            var tenant = ConfigurationManager.AppSettings["ActiveDirectoryTenant"];
            var audience = ConfigurationManager.AppSettings["ActiveDirectoryAudience"];
            var authority = "https://login.microsoftonline.com/common/"; // address for multitenant apps in the public cloud

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions { });

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = authority,
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        // instead of using the default validation we inject our own multitenant validation logic
                        ValidateIssuer = false
                    },
                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        RedirectToIdentityProvider = (context) =>
                        {
                            originalRequestUri = context.Request.Uri;

                            if (IsAjaxRequest(context.Request))
                            {
                                context.HandleResponse();
                                return Task.FromResult(0);
                            }

                            if (context.Request.User.Identity.IsAuthenticated)
                            {
                                // user is already authenticated but authorization failed,
                                // Authorize attribute is redirecting to the login again, we should show unauthorized
                                context.OwinContext.Response.Redirect(ResolveUnauthorizedRedirect(originalRequestUri));
                                context.HandleResponse();
                                return Task.FromResult(0);
                            }

                            // This ensures that the address used for sign in and sign out is picked up dynamically from the request
                            // this allows you to deploy your app without having to change settings
                            // Remember that the base URL of the address used here must be provisioned in Azure AD beforehand.
                            var appBaseUrl = context.Request.Scheme + "://" + context.Request.Host + context.Request.PathBase;
                            context.ProtocolMessage.RedirectUri = appBaseUrl + "/";
                            context.ProtocolMessage.PostLogoutRedirectUri = appBaseUrl;
                            return Task.FromResult(0);
                        },
                        AuthenticationFailed = (context) =>
                        {
                            if (context.Exception != null)
                            {
                                var validationEx = context.Exception as SecurityTokenValidationException;
                                if (validationEx != null)
                                {
                                    Log.Warning("Redirecting to unauthorized view due to failed authorization agents");
                                }
                                else
                                {
                                    Log.Error("Redirecting to unauthorized view due to error: {exception}", context.Exception);
                                }
                            }

                            context.OwinContext.Response.Redirect(ResolveUnauthorizedRedirect(originalRequestUri));
                            context.HandleResponse(); // Suppress the exception
                            return Task.FromResult(0);
                        },
                        AuthorizationCodeReceived = async (context) =>
                        {
                            Log.Information("Authorization code received for user {user}, running authorization agents...", IdentityHelper.GetUsername(context.AuthenticationTicket.Identity));
                            await AuthorizationManager.AuthorizeAsync(context);
                        }
                    }
                });

            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    TokenValidationParameters = new TokenValidationParameters() { ValidAudience = audience, ValidateIssuer = false },
                    Tenant = tenant,
                    AuthenticationType = "ActiveDirectoryBearerAuth",
                    Provider = new OAuthBearerAuthenticationProvider()
                    {
                        OnValidateIdentity = context =>
                        {
                            var identity = context.Ticket.Identity;

                            // Tenant validation
                            var tenantId = identity.ClaimValue(KnownClaimTypes.TenantId);
                            var tenantAuthAgent = new TenantAuthorizationAgent();

                            if (!tenantAuthAgent.IsAllowedTenant(tenantId))
                            {
                                context.Rejected();
                            }

                            identity.AddClaim(new Claim(ClaimTypes.Role, KnownRoles.PowerBIUser));

                            // Ensure name identifier claim is present for the AntiForgery token to use
                            var appId = identity.ClaimValue(KnownClaimTypes.AppId);
                            if (!identity.Claims.Any(c => c.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase)))
                            {
                                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, appId));
                            }

                            return Task.FromResult(0);
                        }
                    }
                });
        }

        private static bool IsAjaxRequest(IOwinRequest request)
        {
            if (request.Query != null && request.Query["X-Requested-With"] == "XMLHttpRequest")
            {
                return true;
            }

            return request.Headers != null && request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        public static string ResolveUnauthorizedRedirect(Uri requestUri)
        {
            var unauthorizedView = "/auth/unauthorized/";

            if (requestUri == null)
            {
                return unauthorizedView;
            }

            var mapping = redirectMapping.SingleOrDefault(kvp => requestUri.AbsolutePath.IndexOf(kvp.Key, StringComparison.OrdinalIgnoreCase) > -1);

            return !string.IsNullOrWhiteSpace(mapping.Value) ? mapping.Value : unauthorizedView;
        }
    }
}