namespace PowerBI.KuduServices
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
    using Newtonsoft.Json;
    using PowerBI.KuduServices.Models;
    using PowerBI.KuduServices.TransientFaultHandling;

    public class KuduService : IKuduService
    {
        private const int TransientFaultRetryAttempts = 3;

        private const string BaseKuduApiUrl = "https://{0}.scm.azurewebsites.net";
        private const string WebJobsListEndpointPattern = "{0}/api/webjobs/";

        private readonly string webSiteName;
        private readonly KuduCredentials credentials;
        private readonly HttpClient httpClient;

        public KuduService(string webSiteName, KuduCredentials credentials) : this(webSiteName, credentials, new HttpClient())
        {
        }

        public KuduService(string webSiteName, KuduCredentials credentials, HttpClient httpClient)
        {
            this.webSiteName = webSiteName;
            this.credentials = credentials;
            this.httpClient = httpClient;
            this.PrepareHttpClientForRequest();
        }

        public async Task<IEnumerable<KuduWebJobDescription>> GetWebJobsAsync()
        {
            var baseKuduWebSiteUrl = string.Format(BaseKuduApiUrl, this.webSiteName);
            var operationUrl = string.Format(WebJobsListEndpointPattern, baseKuduWebSiteUrl);

            var response = await this.ExecuteWithRetryAsync(async () => await this.ExecuteRequestAsync(operationUrl, HttpMethod.Get, null));
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<IEnumerable<KuduWebJobDescription>>(await response.Content.ReadAsStringAsync());
        }

        private static RetryPolicy CreateRetryPolicy()
        {
            var strategy = new ExponentialBackoff(TransientFaultRetryAttempts, RetryStrategy.DefaultMinBackoff, RetryStrategy.DefaultMaxBackoff, RetryStrategy.DefaultClientBackoff);
            return new RetryPolicy<KuduTransientErrorDetectionStrategy>(strategy);
        }

        private void PrepareHttpClientForRequest()
        {
            var authorizationInfo = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", this.credentials.UserName, this.credentials.UserPassword)));
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationInfo);
        }

        private async Task<HttpResponseMessage> ExecuteRequestAsync(string url, HttpMethod method, HttpContent content)
        {
            var response = await this.httpClient.SendAsync(new HttpRequestMessage(method, url) { Content = content }).ConfigureAwait(false);

            return response;
        }

        private async Task<TResult> ExecuteWithRetryAsync<TResult>(Func<Task<TResult>> action)
        {
            try
            {
                var retryPolicy = CreateRetryPolicy();
                return await retryPolicy.ExecuteAsync(action);
            }
            catch (HttpRequestException e)
            {
                var exceptionMessage = string.Format("{0} Requested URI", e.Message);
                throw new HttpRequestException(exceptionMessage);
            }
        }
    }
}
