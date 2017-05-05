namespace PowerBI.Jobs.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Acom.Configuration;
    using Acom.Configuration.Services;
    using Mediation.MessageContracts;
    using Mediation.Queues;

    public class MessageBroadcastService
    {
        private readonly IDictionary<RegionSlot, QueuedMediator> mediators;

        public MessageBroadcastService(ServiceSettings serviceSettings, QueuedMediator.Factory factory)
        {
            var storageConnectionInfos = serviceSettings.ServiceGroups.Single(g => g.Name == "pwrbi-jobs");

            var slots =
                KnownSlots.SlotName == KnownSlots.Unknown ? new[] { KnownSlots.Unknown } : new[] { KnownSlots.Production, KnownSlots.Staging, KnownSlots.Previous };

            mediators = storageConnectionInfos
                .StorageConnections
                .Where(x => !string.Equals(x.Region, KnownSlots.Unknown, StringComparison.OrdinalIgnoreCase))
                .SelectMany(storage => slots.Select(slot => Tuple.Create(new RegionSlot(storage.Region, slot) { Default = storage.Default }, factory.Invoke(storage.ConnectionString, slot))))
                .ToDictionary(x => x.Item1, x => x.Item2);
        }

        public async Task<List<string>> Broadcast<T>(T message, Func<RegionSlot, bool> filter = null) where T : IMediatorCommand
        {
            var errors = new List<string>();

            foreach (var pair in this.mediators.Where(x => filter == null ? true : filter(x.Key)))
            {
                try
                {
                    await pair.Value.Send(message);
                }
                catch (Exception e)
                {
                    var messageStr = string.Format(
                        "An error occurred while sending the message to the queue in region '{0}'. Message: '{1}'. Error details: {2}",
                        pair.Key,
                        message,
                        e.Message);

                    errors.Add(messageStr);
                }
            }

            return errors;
        }

        public class RegionSlot
        {
            public RegionSlot(string region, string slot)
            {
                Region = region;
                Slot = slot;
            }

            public string Region { get; set; }
            public string Slot { get; set; }
            public bool Default { get; set; }

            public override string ToString()
            {
                return string.Format("{0} ({1})", Region, Slot);
            }
        }
    }
}