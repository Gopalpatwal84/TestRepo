using System.Threading.Tasks;
using Mediation;
using Mediation.Handlers;
using PowerBI.Jobs.Forms.Data;
using PowerBI.Jobs.Forms.Events;
using PowerBI.Messages.Commands;
using Serilog;

namespace PowerBI.Jobs.Forms.Handlers
{
    public class SignupRequestHandler : IHandleCommand<SignUpRequestFormMessage>
    {
        private readonly ILogger logger;
        private readonly IMediator mediator;

        public SignupRequestHandler(ILogger logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        public async Task Handle(SignUpRequestFormMessage message)
        {
            this.logger.Information("Persisting signup request data...");

            var user = new User
            {
                Email = message.Email,
                IpAddress = message.IpAddress,
            };

            await this.mediator.Publish(new NewUserEvent(user, message.FormSlug));

            this.logger.Information("Saved signup request with Email {Email}.", message.Email);
        }
    }
}
