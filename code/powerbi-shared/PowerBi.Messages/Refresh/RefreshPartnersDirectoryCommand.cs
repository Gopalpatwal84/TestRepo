using Mediation.MessageContracts;
using Mediation.Queues;

namespace PowerBI.Messages.Refresh
{
    [DestinationQueueName("partners-directory-pump", Description = "Refresh Partners Directory")]
    public class RefreshPartnersDirectoryCommand : IMediatorCommand
    {
        public ReadSource Source { get; set; }
    }
}
