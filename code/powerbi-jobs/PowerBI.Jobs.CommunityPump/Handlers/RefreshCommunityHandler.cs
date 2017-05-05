using System;
using System.Linq;
using System.Threading.Tasks;
using Acom.IO.Collections;
using Acom.Redis;
using Acom.Search.Client;
using Acom.Search.Client.Models;
using Acom.Search.Queue.Indexing;
using Mediation;
using Mediation.Handlers;
using Mediation.Webjobs.Storage;
using PowerBI.Entities;
using PowerBI.Jobs.CommonModules;
using PowerBI.Jobs.CommunityPump.Reader;
using PowerBI.Messages.Refresh;
using PowerBI.Search;
using PowerBI.Search.Documents;
using Serilog;

namespace PowerBI.Jobs.CommunityPump.Handlers
{
    public class RefreshCommunityHandler : IHandleCommand<RefreshCommunityCommand>
    {
        private readonly LithiumReader _lithiumReader;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IRedisClient _redisClient;
        private readonly IWebjobStorageRepository<CommunityMessage> _messageStorage;
        private readonly MediatorBroadcaster _broadcaster;
        private readonly ISearchClient _searchClient;

        public RefreshCommunityHandler(
            LithiumReader lithiumReader,
            ILogger logger,
            IMediator mediator,
            IRedisClient redisClient,
            IWebjobStorageRepository<CommunityMessage> messageStorage,
            MediatorBroadcaster broadcaster,
            ISearchClient searchClient)
        {
            _lithiumReader = lithiumReader;
            _logger = logger;
            _mediator = mediator;
            _redisClient = redisClient;
            _messageStorage = messageStorage;
            _broadcaster = broadcaster;
            _searchClient = searchClient;
        }

        public async Task Handle(RefreshCommunityCommand command)
        {
            if (command != null && command.Source == ReadSource.Remote)
            {
                await ReadFromApi();
            }
            else
            {
                await ReadFromStorage();
            }
        }

        private async Task ReadFromApi()
        {
            Exception delayedException = null;
            try
            {
                _logger.Verbose("Retrieving community messages from Lithium");

                var messages = await _lithiumReader.GetCommunityMessagesAsync();

                if (!messages.Any())
                {
                    _logger.Warning("No messages found, cancelling task");
                    return;
                }

                _logger.Information("{MessagesCount} messages fetched", messages.Count());

                _logger.Verbose("Saving messages to storage");

                await _messageStorage.SaveResults(messages);
            }
            catch (Exception ex)
            {
                delayedException = ex;
            }
            await EnsureOtherInstancesAreNotified();
            if (delayedException != null) throw delayedException;
        }

        private async Task ReadFromStorage()
        {
            var latest = _messageStorage.LoadResults().Result?.OrderByDescending(x => x.LastEditTime);

            if (latest != null)
            {
                _logger.Verbose("Setting community messages in redis");

                await _redisClient.SetAllAsync(latest);

                _logger.Verbose("Queuing community messages to update the Azure Search index...");

                var messagesToIndex = latest.AsQueryable();

                var latestSearchEntry = await this.GetLatestCommunityEntryAsync();
                if (latestSearchEntry != null)
                {
                    messagesToIndex = messagesToIndex.Where(m => m.LastEditTime >= latestSearchEntry.Modified);
                }

                await messagesToIndex.ForEachAsync(async message =>
                {
                    var entry = this.CreateEntry(message);
                    await _mediator.Send(new IndexItemCommand(entry, IndexAction.UpdateOrInsert));
                });

                _logger.Verbose("Done");
            }
        }

        private async Task<CommunityEntry> GetLatestCommunityEntryAsync()
        {
            var search = await _searchClient.CreateQuery<CommunityEntry>()
                .OrderByPropertyDescending(x => x.Modified)
                .Top(1)
                .SearchAsync();

            return search.Results.Select(x => x as AcomSearchResult<CommunityEntry>)
                .FirstOrDefault(x => x != null)
                ?.Document;
        }

        private async Task EnsureOtherInstancesAreNotified()
        {
            await _broadcaster.Send(new RefreshCommunityCommand { Source = ReadSource.Storage });
        }

        private CommunityEntry CreateEntry(CommunityMessage message)
        {
            var entry = new CommunityEntry(message.Url, message.Slug)
            {
                Title = message.Subject,
                Summary = message.SearchSnippet,
                Created = message.PostTime,
                Modified = message.LastEditTime,
                Content = message.Body
            };

            return entry;
        }
    }
}
