using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acom.IO;
using Acom.IO.Collections;
using Acom.Redis;
using Acom.Search.Client.Documents;
using Acom.Search.Queue.Indexing;
using Mediation;
using Mediation.Handlers;
using Mediation.Webjobs.Storage;
using PowerBI.Entities;
using PowerBI.Entities.Partners;
using PowerBI.Jobs.CommonModules;
using PowerBI.Jobs.PartnersShowcasePump.Features;
using PowerBI.Messages.Refresh;
using PowerBI.Search.Documents;
using Serilog;

namespace PowerBI.Jobs.PartnersShowcasePump.Handlers
{
    class RefreshPartnersShowcaseHandler : IHandleCommand<RefreshPartnersShowcaseCommand>
    {
        private readonly IRedisClient _redisClient;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IWebjobStorageRepository<PartnerShowcase> _partnerStorage;
        private readonly MediatorBroadcaster _broadcaster;
        private readonly PartnersShowcaseReader _partnersReader;
        private readonly IRepository<Curation<PartnerShowcase>> _curationsRepository;

        public RefreshPartnersShowcaseHandler(
            IRedisClient redisClient,
            ILogger logger,
            IMediator mediator,
            IWebjobStorageRepository<PartnerShowcase> partnerStorage,
            MediatorBroadcaster broadcaster,
            PartnersShowcaseReader partnersReader,
            IRepository<Curation<PartnerShowcase>> curationsRepository)
        {
            _redisClient = redisClient;
            _logger = logger;
            _mediator = mediator;
            _partnerStorage = partnerStorage;
            _broadcaster = broadcaster;
            _partnersReader = partnersReader;
            _curationsRepository = curationsRepository;
        }

        public async Task Handle(RefreshPartnersShowcaseCommand command)
        {
            if (command != null && command.Source == ReadSource.Remote)
            {
                Exception delayedException = null;
                try
                {
                    _logger.Verbose("Retrieving partners");
                    var partners = await _partnersReader.Fetch();

                    if (!partners.Any())
                    {
                        _logger.Warning("No partners retrieved. Cancelling task");
                        return;
                    }

                    _logger.Information("{PartnersCount} partners fetched from {ReadSource}", partners.Length, command.Source);

                    _logger.Verbose("Saving to storage");
                    await _partnerStorage.SaveResults(partners);
                }
                catch (Exception ex)
                {
                    delayedException = ex;
                }
                await _broadcaster.Send(new RefreshPartnersShowcaseCommand { Source = ReadSource.Storage });
                if (delayedException != null) throw delayedException;
            }
            else
            {
                _logger.Verbose("Reading from storage");
                var partners = await _partnerStorage.LoadResults();

                if (partners?.Any() == true)
                {
                    _logger.Information("{PartnersCount} partners fetched from storage", partners.Length);

                    _logger.Verbose("Setting in redis");
                    await _redisClient.SetAllAsync(partners.OrderBy(x => x.Slug));

                    _logger.Verbose("Queuing to update the Azure Search index");
                    var curationSlugs = await _curationsRepository.GetAsync("partner-showcase");
                    await partners.ForEachAsync(async x =>
                    {
                        var indexItem = CreateIndexItem(x, IndexAction.UpdateOrInsert, curationSlugs.ItemSlugs);
                        await _mediator.Send(indexItem);
                    });
                }
                else
                {
                    _logger.Warning("No partners fetched from storage");
                }
            }
        }

        private IndexItemCommand CreateIndexItem(PartnerShowcase entity, IndexAction action, IEnumerable<string> curationSlugs)
        {
            var defaultProfile = entity.DefaultProfile();
            var title = $"{defaultProfile.Name} - {defaultProfile.SolutionName}";

            var partner = new PartnerShowcaseProfileEntry(entity.Url, entity.Slug)
            {
                Title = title,
                TitleCanonical = title.ToLowerInvariant(),
                Summary = defaultProfile.ShortDescription,
                Cultures = entity.Profiles.Select(p => p.Culture).Distinct(),
                ExternalId = defaultProfile.PartnerId,
                Countries = entity.Profiles.SelectMany(p => p.Countries.Select(c => c.Slug)).Distinct().ToArray(),
                Industries = entity.Profiles.SelectMany(p => p.Industries.Select(s => s.Slug)).Distinct().ToList(),
                Departments = entity.Profiles.SelectMany(p => p.Departments.Select(s => s.Slug)).Distinct().ToArray(),
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
                Featured = curationSlugs.Contains(entity.Slug),
                ExtraPartnerData = new PartnerShowcaseProfileEntry.ExtraData
                {
                    LogoUrls = entity.Profiles.ToDictionary(p => p.Culture, p => p.LogoUrl)
                }
            };

            foreach (var profile in entity.Profiles.Where(p => !p.Culture.Equals("en-us", StringComparison.InvariantCultureIgnoreCase)))
            {
                if (LanguageDictionary.SupportedSlugs.ContainsKey(profile.Culture))
                {
                    partner.TitleLanguages[profile.Culture] = $"{profile.Name} - {profile.SolutionName}";
                    partner.SummaryLanguages[profile.Culture] = profile.ShortDescription;
                }
            }

            return new IndexItemCommand(partner, action);
        }
    }
}
