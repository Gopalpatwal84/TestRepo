using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Mediation.MessageContracts;
using Mediation.Webjobs.Interceptor;
using Serilog;

namespace PowerBI.Jobs.CommonModules.Instrumentation
{
    public class LoggingInterceptor : IInboundInterceptor
    {
        private readonly ILogger logger;
        private readonly Stopwatch timer = new Stopwatch();
        private static readonly Task Done = Task.FromResult(true);

        public LoggingInterceptor(ILogger logger)
        {
            this.logger = logger;
        }

        public int Priority { get { return 0; } }

        public Task OnCommandHandlerExecuting<TCommand>(TCommand command) where TCommand : IMediatorCommand
        {
            timer.Start();
            return Done;
        }

        public Task OnCommandHandlerSuccess<TCommand>(TCommand command) where TCommand : IMediatorCommand
        {
            timer.Stop();
            logger.Debug("{CommandType} was handled in {CommandTime}", typeof(TCommand).Name, timer.Elapsed);
            return Done;
        }

        public void OnCommandHandlerError<TCommand>(TCommand command, Exception exception, ref bool handled) where TCommand : IMediatorCommand
        {
            timer.Stop();
            logger.Error(exception, "Failed when handling {CommandType}. Time elapsed: {CommandTime}", typeof(TCommand).Name, timer.Elapsed);
        }
    }
}
