namespace PowerBI.Web.Infrastructure.Mediator
{
    using System;
    using System.ComponentModel.Composition;
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Web;
    using Acom.Configuration;
    using Mediation.Interceptors;
    using PowerBI.Messages.Models;
    
    public class MessageDiagnosticInterceptor : OutboundInterceptorBase
    {
        public async override Task OnCommandSending<TCommand>(TCommand command)
        {
            var diagnostics = command as IDiagnosticInfo;
            if (diagnostics != null)
            {
                string userHostAddress = null;
                try
                {
                    userHostAddress = HttpContext.Current.Request.UserHostAddress;
                }
                catch { }
                diagnostics.DiagnosticsInfo = new DiagnosticsInfo
                {
                    SentTimestamp = DateTime.UtcNow,
                    Source = "PowerBI",
                    SourceRegion = KnownAntaresVariables.RegionName,
                    SourceSiteName = KnownAntaresVariables.SiteName,
                    InstanceId = KnownAntaresVariables.InstanceId,
                    SlotName = KnownSlots.SlotName,
                    DeploymentVersion = KnownDeploymentVariables.DeploymentVersion, 
                    SourceIp = userHostAddress
                };
                if (diagnostics.MessageId == Guid.Empty) diagnostics.MessageId = Guid.NewGuid();
                if (diagnostics.TransactionId == Guid.Empty) diagnostics.TransactionId = Guid.NewGuid();
                if (diagnostics.TransactionStarted == DateTime.MinValue) diagnostics.TransactionStarted = Clock.UtcNow().DateTime;
            }
        }
    }
}