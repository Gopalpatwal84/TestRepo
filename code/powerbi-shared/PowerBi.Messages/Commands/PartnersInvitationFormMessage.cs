namespace PowerBI.Messages.Commands
{
    using Mediation.Queues;

    [DestinationQueueName("partners-invitation")]
    public class PartnersInvitationFormMessage : RegistrationRequestFormMessage
    {
        public string CompanyName { get; set; }

        public string MpnId { get; set; }
    }
}
