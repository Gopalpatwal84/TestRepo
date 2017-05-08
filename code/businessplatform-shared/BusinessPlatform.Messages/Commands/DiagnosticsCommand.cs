namespace BusinessPlatform.Messages.Commands
{
    using System;
    using BusinessPlatform.Messages.Models;
    using Mediation.MessageContracts;
    using Newtonsoft.Json;

    public abstract class DiagnosticsCommand : IMediatorCommand, IDiagnosticInfo
    {
        [JsonProperty]
        Guid IDiagnosticInfo.MessageId { get; set; }

        [JsonProperty]
        DiagnosticsInfo IDiagnosticInfo.DiagnosticsInfo { get; set; }

        [JsonProperty]
        Guid IMessageTransaction.TransactionId { get; set; }

        [JsonProperty]
        DateTime IMessageTransaction.TransactionStarted { get; set; }
    }
}