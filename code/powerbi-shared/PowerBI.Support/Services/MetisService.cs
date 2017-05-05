using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PowerBI.Support.Helpers;
using PowerBI.Support.Models;
using PowerBI.Support.TransientErrorDetectionStrategies;
using Serilog;

namespace PowerBI.Support.Services
{
    public class MetisService
    {
        private readonly AuthToken _authToken;
        private readonly Configuration _configuration;

        public MetisService(
            AuthToken authToken,
            Configuration configuration)
        {
            _authToken = authToken;
            _configuration = configuration;
        }

        public async Task<string> SubmitSupportTicketAsync(MetisIncident incident)
        {
            var jsonPayload = JsonConvert.SerializeObject(incident);
            var trackingId = Guid.NewGuid().ToString();
            var endpointUrl = string.Format(_configuration.MetisAPIUrl, incident.Organization.TenantId);

            var jsonFormatter = new JsonMediaTypeFormatter()
            {
                SerializerSettings =
                    new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                        TypeNameAssemblyFormat = FormatterAssemblyStyle.Full
                    }
            };

            using (var handler = new HttpClientHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var accessToken = await _authToken.GetAuthToken();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    client.DefaultRequestHeaders.Add("x-ms-correlation-id", trackingId);

                    var request = JsonConvert.DeserializeObject<JObject>(jsonPayload);

                    var response = await this.ExecuteWithRetryAsync(async () => await client.PostAsync(endpointUrl, request, jsonFormatter));

                    var content = await response.Content.ReadAsStringAsync();

                    var ticketId = string.Empty;

                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        ticketId = content.Trim('"');
                        Log.Information("Incident correctly submitted on Metis. Incident ID: {0}", ticketId);
                    }
                    else
                    {
                        Log.Error("There was an error submitting the incident in Metis. Status Code: {0} - Content: {1}", response.StatusCode, content);
                    }

                    return ticketId;
                }
            }
        }

        private async Task<TResult> ExecuteWithRetryAsync<TResult>(Func<Task<TResult>> action)
        {
            var retryPolicy = this.CreateRetryPolicy();

            return await retryPolicy.ExecuteAsync(
                async () =>
                {
                    try
                    {
                        return await action();
                    }
                    catch (TaskCanceledException e) // HttpClient throws this on timeout
                    {
                        // we need to convert it to a different exception
                        // otherwise ExecuteAsync will think we requested cancellation
                        throw new HttpRequestException("Request timed out", e);
                    }
                });
        }

        private RetryPolicy CreateRetryPolicy()
        {
            var strategy = new ExponentialBackoff(RetryStrategy.DefaultClientRetryCount, RetryStrategy.DefaultMinBackoff, RetryStrategy.DefaultMaxBackoff, RetryStrategy.DefaultClientBackoff);
            return new RetryPolicy<MetisTransientErrorDetectionStrategy>(strategy);
        }
    }
}
