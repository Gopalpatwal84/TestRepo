using Mediation.Queues;

namespace PowerBI.Messages.Commands
{
    [DestinationQueueName("support-ticket")]
    public class SupportTicketFormMessage : DiagnosticsCommand
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public string FormSlug { get; set; }
    }
}
