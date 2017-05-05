using Mediation.MessageContracts;
using Mediation.Queues;

namespace PowerBI.Messages.Refresh
{
    [DestinationQueueName("community-pump", Description = "Refresh Community")]
    public class RefreshCommunityCommand : IMediatorCommand
    {
        public ReadSource Source { get; set; }
    }
}
