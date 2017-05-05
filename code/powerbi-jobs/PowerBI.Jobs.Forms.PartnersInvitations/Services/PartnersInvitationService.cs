namespace PowerBI.Jobs.Forms.PartnersInvitations.Services
{
    using System.Threading.Tasks;
    using Onyx.Client;
    using Onyx.Client.Extensions.Partners;
    using Onyx.Contracts.Partners;
    using PowerBI.Messages.Commands;
    using Serilog;

    public class PartnersInvitationService
    {
        private readonly IOnyxRequestClient client;
        private readonly ILogger logger;

        public PartnersInvitationService(ILogger logger, IOnyxRequestClient client)
        {
            this.logger = logger;
            this.client = client;
        }

        public async Task SendInvitation(PartnersInvitationFormMessage message)
        {
            this.logger.Verbose("Sending nomination request to partners portal for Email {email}", message.Email);

            var contract = new NominationContract();
            contract.Country = message.Country;
            contract.Email = message.Email;
            contract.FirstName = message.FirstName;
            contract.LastName = message.LastName;
            contract.MpnId = message.MpnId;
            contract.OrganizationName = message.CompanyName;
            contract.Role = message.JobTitle;
            contract.Tenant = "PowerBI";

            try
            {
                await this.client.NominateAsync(contract);
                this.logger.Verbose("Nomination request sent ({email})", message.Email);
            }
            catch (System.Exception ex)
            {
                this.logger.Error("Nomination request failed ({email}). Exception: {exceptionDetails}", message.Email, ex.ToString());
                throw;
            }
        }
    }
}
