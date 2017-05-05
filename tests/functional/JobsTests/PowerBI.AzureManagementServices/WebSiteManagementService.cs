namespace PowerBI.AzureManagementServices
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Management.WebSites;
    using PowerBI.AzureManagementServices.Models;

    public class WebSiteManagementService : IWebSiteManagementService
    {
        private readonly string certThumprint;
        private readonly string subscriptionId;

        public WebSiteManagementService(string subscriptionId, string certThumprint)
        {
            this.subscriptionId = subscriptionId;
            this.certThumprint = certThumprint;
        }

        public async Task<IEnumerable<WebSitePublishProfile>> GetWebSitePublishProfileAsync(IEnumerable<string> webSiteNames)
        {
            var publishProfiles = new List<WebSitePublishProfile>();
            var credentials = this.GetCredentials();

            using (var client = new WebSiteManagementClient(credentials))
            {
                var webSpacesListResponse = await client.WebSpaces.ListAsync();

                foreach (var webSpace in webSpacesListResponse)
                {
                    // Get all WebSites associated with the webspace
                    var webSitesResponse = await client.WebSpaces.ListWebSitesAsync(webSpace.Name, null);

                    var webSites = webSitesResponse.WebSites.Where(ws => webSiteNames.Contains(ws.Name));

                    foreach (var webSite in webSites)
                    {
                        var profiles = await client.WebSites.GetPublishProfileAsync(webSpace.Name, webSite.Name);
                        var profile = profiles.FirstOrDefault();    // Take the first profile, as all of them have the same username and password

                        if (profile != null)
                        {
                            publishProfiles.Add(new WebSitePublishProfile { WebSiteName = webSite.Name, UserName = profile.UserName, UserPassword = profile.UserPassword });
                        }
                    }
                }
            }

            return publishProfiles;
        }

        public CertificateCloudCredentials GetCredentials()
        {
            X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, this.certThumprint, false);

            return new CertificateCloudCredentials(this.subscriptionId, certCollection[0]);
        }
    }
}
