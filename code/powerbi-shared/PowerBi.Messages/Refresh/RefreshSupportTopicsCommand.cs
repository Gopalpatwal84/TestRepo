using Mediation.MessageContracts;
using Mediation.Queues;

namespace PowerBI.Messages.Refresh
{
    [DestinationQueueName("support-topics-pump", Description = "Refresh Support Topics")]
    public class RefreshSupportTopicsCommand : IMediatorCommand
    {
        public bool RunFullUpdate { get; set; }
    }
}
