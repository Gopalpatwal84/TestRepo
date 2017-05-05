using Marketo.API.Leads;
using Mediation;
using Mediation.Handlers;
using PowerBI.Jobs.Forms.Data;
using PowerBI.Jobs.Forms.Events;
using PowerBI.Messages.Commands;
using PowerBI.Messages.Models;
using System.Threading.Tasks;

namespace PowerBI.Jobs.Forms.Handlers
{
    public class RegistrationRequestHandler : IHandleCommand<RegistrationRequestFormMessage>
    {
        private readonly IMediator mediator;

        public RegistrationRequestHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task Handle(RegistrationRequestFormMessage message)
        {
            if (!string.IsNullOrEmpty(message.MarketoResourceId))
            {
                var lead = new Lead
                {
                    FirstName = message.FirstName,
                    LastName = message.LastName,
                    Title = message.JobTitle,
                    Email = message.Email,
                    Country = message.Country,
                    Phone = message.Phone,
                    MarketoResourceId = message.MarketoResourceId,
                    MarketoTimeStamp = message.DiagnosticInfo(x => x.TransactionStarted),
                    TrackingCookie = message.TrackingCookie,
                };

                await this.mediator.Publish(new NewMarketoLeadEvent(lead, message.Consent));
            }

            var user = new User
            {
                FirstName = message.FirstName,
                LastName = message.LastName,
                JobTitle = message.JobTitle,
                Email = message.Email,
                Country = message.Country,
                Phone = message.Phone,
            };

            await this.mediator.Publish(new NewUserEvent(user, message.FormSlug));
        }
    }
}
