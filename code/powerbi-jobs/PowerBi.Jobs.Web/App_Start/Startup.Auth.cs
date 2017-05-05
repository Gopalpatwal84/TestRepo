namespace PowerBI.Jobs.Web
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IdentityModel.Tokens;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.ActiveDirectory;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.OpenIdConnect;
    using Newtonsoft.Json;
    using Owin;

    public partial class Startup
    {
        private IEnumerable<Tenant> tenants;

        public void ConfigureAuth(IAppBuilder app)
        {
            var clientId = ConfigurationManager.AppSettings["ActiveDirectoryClientID"];
            var tenant = ConfigurationManager.AppSettings["ActiveDirectoryTenant"];
            var audience = ConfigurationManager.AppSettings["ActiveDirectoryAudience"];
            var authority = "https://login.microsoftonline.com/common/"; // address for multitenant apps in the public cloud

            this.ConfigureTenants();

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
                            if (IsAjaxRequest(context.Request))
                            {
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
                        SecurityTokenValidated = (context) => // we use this notification for injecting our custom logic
                        {
                            // retriever caller data from the incoming principal
                            var tenantID = context.AuthenticationTicket.Identity.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

                            // tenant validation
                            if (!tenants.Any(t => string.Equals(t.TenantID, tenantID, StringComparison.OrdinalIgnoreCase)))
                            {
                                throw new SecurityTokenValidationException();
                            }

                            return Task.FromResult(0);
                        },
                        AuthenticationFailed = (context) =>
                        {
                            context.OwinContext.Response.Redirect("/auth/unauthorized/");
                            context.HandleResponse(); // Suppress the exception
                            return Task.FromResult(0);
                        }
                    }
                });

            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
              new WindowsAzureActiveDirectoryBearerAuthenticationOptions
              {
                  TokenValidationParameters = new TokenValidationParameters() { ValidAudience = audience },
                  Tenant = tenant,
                  AuthenticationType = "ActiveDirectoryBearerAuth"
              });
        }

        private void ConfigureTenants()
        {
            var tenantsList = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/tenants.json"));
            this.tenants = JsonConvert.DeserializeObject<List<Tenant>>(tenantsList);
        }

        private static bool IsAjaxRequest(IOwinRequest request)
        {
            if (request.Query != null && request.Query["X-Requested-With"] == "XMLHttpRequest")
            {
                return true;
            }

            return request.Headers != null && request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private class Tenant
        {
            public string TenantID { get; set; }

            public string Organization { get; set; }

            public string Comments { get; set; }
        }
    }
}