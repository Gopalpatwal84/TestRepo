namespace BusinessPlatform.Jobs.CommonModules.Instrumentation
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using BusinessPlatform.Messages.Models;
    using Mediation.Queues;
    using Mediation.Webjobs.Interceptor;

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
                this.messageInstrumentationService.SaveOrUpdateInstrumentationInfo(diag, DateTime.UtcNow, fromQueue);
            }
        }
    }
}
