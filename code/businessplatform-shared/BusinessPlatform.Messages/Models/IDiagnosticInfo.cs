namespace BusinessPlatform.Messages.Models
{
    using System;

    public interface IDiagnosticInfo : IMessageTransaction
    {
        Guid MessageId { get; set; }

        DiagnosticsInfo DiagnosticsInfo { get; set; } 
    }
}