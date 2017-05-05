using System.Linq;
using System.Threading.Tasks;
using Acom.Configuration.Services;
using Acom.IO.Collections;
using Mediation.MessageContracts;
using Mediation.Queues;

namespace PowerBI.Jobs.CommonModules
{
    public class MediatorBroadcaster
    {
        private readonly QueuedMediator[] _globalMediators;

        public MediatorBroadcaster(QueuedMediator.Factory mediatorFactory, ServiceSettings settings)
        {
            var jobStorage = settings.ServiceGroups.Single(x => x.Name == "pwrbi-jobs")
                .StorageConnections
                .Select(x => x.ConnectionString)
                .Distinct()
                .ToArray();
            _globalMediators = jobStorage.Select(x => mediatorFactory.Invoke(x)).ToArray();
        }

        public async Task Send<TMediatorCommand>(TMediatorCommand mediatorCommand) where TMediatorCommand : IMediatorCommand
        {
            await _globalMediators.ForEachAsync(mediator => mediator.Send(mediatorCommand));
        }
    }
}
