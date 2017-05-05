using System.Threading.Tasks;
using Mediation;
using Mediation.Handlers;
using PowerBI.Jobs.Forms.Data;
using PowerBI.Jobs.Forms.Events;
using PowerBI.Messages.Commands;
using Serilog;

namespace PowerBI.Jobs.Forms.Handlers
{
    public class SupportTicketHandler : IHandleCommand<SupportTicketFormMessage>
    {
        private readonly ILogger logger;
        private readonly IMediator mediator;

        public SupportTicketHandler(ILogger logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        public async Task Handle(SupportTicketFormMessage message)
        {
            this.logger.Information("Persisting support form data...");

            var user = new User
            {
                FirstName = message.FirstName,
                LastName = message.LastName,
                Email = message.Email,
                Phone = message.Phone,
                Country = message.Country
            };

            await this.mediator.Publish(new NewUserEvent(user, message.FormSlug));

            this.logger.Information("Saved support form with Email {Email}.", message.Email);
        }
    }
}
