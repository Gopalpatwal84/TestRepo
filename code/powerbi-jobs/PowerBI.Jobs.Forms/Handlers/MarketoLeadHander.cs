using System.Threading.Tasks;
using Marketo.API;
using Marketo.API.Identity;
using Marketo.API.Leads;
using Mediation.Handlers;
using PowerBI.Jobs.Forms.Events;
using Serilog;

namespace PowerBI.Jobs.Forms.Handlers
{

    public class MarketoLeadHander : IHandleCompetingEvent<NewMarketoLeadEvent>
    {
        private static readonly object LockObject = new object();

        private readonly ILogger logger;

        public MarketoLeadHander(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task Handle(NewMarketoLeadEvent mediatorEvent)
        {
            this.logger.Information("Saving lead in Marketo...");
            var configuration = new MarketoConfiguration();

            if (configuration.ClientId != "{clientId}" && configuration.ClientSecret != "{clientSecret}")
            {
                lock (LockObject)
                {
                    if (!IdentityService.IsAuthenticated())
                    {
                        var identityService = new IdentityService(configuration);
                        identityService.Authenticate();
                    }
                }

                var marketoService = new LeadService(configuration, this.logger);
                var marketoResponse = await marketoService.CreateOrUpdateLeadAsync(mediatorEvent.Lead);
                this.logger.Information("Marketo Lead with request ID {MarketoLeadId} saved.", marketoResponse.RequestId);
            }
        }
    }
}
