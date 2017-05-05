using Mediation.Queues;
using PowerBI.Messages.Commands;

namespace PowerBI.Messages.Refresh
{
    [DestinationQueueName("blog-pump-full")]
    public class RefreshFullBlogCommand : DiagnosticsCommand
    {
        public ReadSource ReadSource { get; set; }
    }
}
