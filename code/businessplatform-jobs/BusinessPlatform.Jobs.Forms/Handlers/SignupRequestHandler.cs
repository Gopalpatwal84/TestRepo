namespace BusinessPlatform.Jobs.Forms.Handlers
{
    using System.Threading.Tasks;
    using Acom.DocumentDb;
    using BusinessPlatform.Jobs.Forms.Data;
    using BusinessPlatform.Messages.Commands;
    using Mediation.Handlers;
    using Serilog;

    public class SignupRequestHandler : IHandleCommand<SignUpRequestFormMessage>
    {
        private readonly ILogger logger;
        private readonly IDocumentDbClient repository;

        public SignupRequestHandler(ILogger logger, IDocumentDbClient repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        public async Task Handle(SignUpRequestFormMessage message)
        {
            this.logger.Information("Persisting signup request data...");

            var slug = message.Email.ToLowerInvariant();
            var user = (await this.repository.GetAsync<User>(slug)) ?? new User();

            user.Slug = message.Email.ToLowerInvariant();
            user.Email = message.Email;
            user.Date = message.Date;

            if (!string.IsNullOrEmpty(message.FormSlug) && !user.Forms.Contains(message.FormSlug))
            {
                user.Forms.Add(message.FormSlug);
            }

            await this.repository.SetAsync(user, fullReplace: true);

            this.logger.Information("Saved signup request with Slug {signupRequestSlug}.", user.Slug);
        }
    }
}
