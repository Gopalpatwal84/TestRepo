using Mediation.MessageContracts;
using Mediation.Queues;

namespace PowerBI.Messages.Refresh
{
    [DestinationQueueName("articles-pump", Description = "Refresh Articles")]
    public class RefreshArticlesCommand : IMediatorCommand
    {
    }
}
