namespace PowerBI.Jobs.Forms.PartnersInvitations
{
    using System.Threading.Tasks;
    using Mediation.Handlers;
    using PowerBI.Jobs.Forms.PartnersInvitations.Services;
    using PowerBI.Messages.Commands;

    public class PartnersInvitationsHandler : IHandleCommand<PartnersInvitationFormMessage>
    {
        private readonly PartnersInvitationService service;

        public PartnersInvitationsHandler(PartnersInvitationService service)
        {
            this.service = service;
        }

        public async Task Handle(PartnersInvitationFormMessage message)
        {
            await this.service.SendInvitation(message);
        }
    }
}
