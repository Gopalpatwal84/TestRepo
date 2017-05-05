using Mediation.MessageContracts;
using Mediation.Queues;

namespace PowerBI.Messages.Refresh
{
    [DestinationQueueName("partners-showcase-pump", Description = "Refresh Partners Showcase")]
    public class RefreshPartnersShowcaseCommand : IMediatorCommand
    {
        public ReadSource Source { get; set; }
    }
}
