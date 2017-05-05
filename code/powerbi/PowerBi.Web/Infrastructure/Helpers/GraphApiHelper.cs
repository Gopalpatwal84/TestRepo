using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;

namespace PowerBI.Web.Infrastructure.Helpers
{
    public class GraphApiHelper
    {
        private static string graphBaseUrl = ConfigurationManager.AppSettings["ActiveDirectoryGraphUrl"];

        private static string authority = "https://login.microsoftonline.com/common/";

        public static async Task<string> GetAccessTokenAsync(string accessCode, Uri redirectUri)
        {
            var clientId = ConfigurationManager.AppSettings["ActiveDirectoryClientID"];
            var clientSecret = ConfigurationManager.AppSettings["ActiveDirectoryPassword"];

            try
            {
                var authContext = new AuthenticationContext(authority);
                var credential = new ClientCredential(clientId, clientSecret);
                var result = await authContext.AcquireTokenByAuthorizationCodeAsync(accessCode, redirectUri, credential, graphBaseUrl);

                return result.AccessToken;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public static async Task<UserProfile> GetUserProfileAsync(string accessToken)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await httpClient.GetAsync(string.Format("{0}/me?api-version=1.6", graphBaseUrl));
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return await Task.Run(() => JsonConvert.DeserializeObject<UserProfile>(jsonString));
                }
            }
            catch(Exception)
            {
            }

            return null;
        }
    }

    public class UserProfile
    {
        public string ObjectId { get; set; }

        public string UserPrincipalName { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public string DisplayName { get; set; }

        public string PreferredLanguage { get; set; }

        public string Mail { get; set; }

        public string JobTitle { get; set; }

        public string CompanyName { get; set; }

        public AssignedPlan[] AssignedPlans { get; set; }
    }

    public class AssignedPlan
    {
        public string ServicePlanId { get; set; }
    }
}