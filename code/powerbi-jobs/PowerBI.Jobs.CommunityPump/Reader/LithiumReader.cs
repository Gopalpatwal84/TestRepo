using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Acom.IO.Collections;
using Acom.IO.Json;
using PowerBI.Entities;
using PowerBI.Jobs.CommonModules.TransientErrorDetection;
using PowerBI.Jobs.CommunityPump.Models;
using Serilog;

namespace PowerBI.Jobs.CommunityPump.Reader
{
    public class LithiumReader
    {
        private const int MaxParallelRequests = 5;
        private const int MessageListLimit = 500;
        private const int MessageListOffset = 500;

        private const string MessageCountLiql = "SELECT count(*) FROM messages";
        private const string MessageListLiqlPattern = "SELECT id,subject,search_snippet,body,view_href,post_time,current_revision.last_edit_time FROM messages ORDER BY post_time ASC LIMIT {0} OFFSET {1}";

        private readonly Configuration configuration;
        private readonly ILogger logger;

        public LithiumReader(Configuration configuration, ILogger logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task<IEnumerable<CommunityMessage>> GetCommunityMessagesAsync()
        {
            var messageCount = await this.GetMessageCountAsync();
            var pages = Math.Ceiling(messageCount / (double)MessageListOffset);
            var messages = new ConcurrentBag<LithiumApiMessageData>();

            await Enumerable.Range(1, (int)pages).ForEachAsync(async page =>
            {
                var pageMessages = await this.GetMessagesAsync(page);
                foreach (var message in pageMessages)
                {
                    messages.Add(message);
                }
            }, 
            initialRequestCount: MaxParallelRequests, 
            maxRequestCount: MaxParallelRequests);

            return messages
                .OrderByDescending(m => m.CurrentRevision.LastEditTime)
                .Select(m => new CommunityMessage
                {
                    Slug = m.Id,
                    Url = m.ViewHref,
                    Subject = m.Subject,
                    SearchSnippet = WebUtility.HtmlDecode(m.SearchSnippet),
                    Body = m.Body,
                    LastEditTime = m.CurrentRevision.LastEditTime,
                    PostTime = m.PostTime
                });
        }

        private async Task<int> GetMessageCountAsync()
        {
            var result = await this.ExecuteJsonRequestAsync<LithiumApiResponse<LithiumApiCountData>>(this.GetLiqlSearchApiUrl(MessageCountLiql));
            return result.Data.Count;
        }

        private async Task<IEnumerable<LithiumApiMessageData>> GetMessagesAsync(int page)
        {
            var messagesLiql = string.Format(MessageListLiqlPattern, MessageListLimit, MessageListOffset * (page - 1));
            var result = await this.ExecuteJsonRequestAsync<LithiumApiResponse<LithiumApiMessageListData>>(this.GetLiqlSearchApiUrl(messagesLiql));
            return result?.Data?.Items;
        }

        private string GetLiqlSearchApiUrl(string liql)
        {
            var apiUrl = string.Format("{0}/{1}", this.configuration.LithiumApiEndpoint, this.configuration.LithiumApiTenantId);
            var encodedLiql = WebUtility.UrlEncode(liql);

            return string.Format("{0}/search?q={1}", apiUrl, encodedLiql);
        }

        private Task<TResult> ExecuteJsonRequestAsync<TResult>(string operationUrl)
        {
            var jsonRequest = new JsonGetRequest(new Uri(operationUrl));
            jsonRequest.CustomHeaders.Add("client-id", this.configuration.LithiumApiClientId);
            var jsonClient = new JsonRequestClient();

            return this.ExecuteWithRetryAsync(async () => await jsonClient.ExecuteAsync<TResult>(jsonRequest));
        }

        private async Task<TResult> ExecuteWithRetryAsync<TResult>(Func<Task<TResult>> action)
        {
            var retryPolicy = RetryPolicyFactory.CreateHttpClientTransientErrorDetectionPolicy();

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
    }
}
