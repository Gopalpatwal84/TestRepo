namespace PowerBI.Jobs.CommonModules.Instrumentation
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Mediation.Queues;
    using Mediation.Webjobs.Interceptor;
    using PowerBI.Messages.Models;

    public class MessageInstrumentationInterceptor : InboundInterceptorBase
    {
        private readonly IMessageInstrumentationService messageInstrumentationService;

        public MessageInstrumentationInterceptor(IMessageInstrumentationService messageInstrumentationService)
        {
            this.messageInstrumentationService = messageInstrumentationService;
        }

        public async override Task OnCommandHandlerExecuting<TCommand>(TCommand command)
        {
            var diag = command as IDiagnosticInfo;
            if (diag != null)
            {
                var fromQueue = command.GetType().GetCustomAttribute<DestinationQueueNameAttribute>().DestinationQueueBySlot();
                messageInstrumentationService.SaveOrUpdateInstrumentationInfo(diag, DateTime.UtcNow, fromQueue);
            }
        }
    }
}
