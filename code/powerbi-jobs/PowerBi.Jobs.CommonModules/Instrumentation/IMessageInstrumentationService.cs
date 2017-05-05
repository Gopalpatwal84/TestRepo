namespace PowerBI.Jobs.CommonModules.Instrumentation
{
    using System;
    using PowerBI.Messages.Models;

    public interface IMessageInstrumentationService
    {
        void SaveOrUpdateInstrumentationInfo(IDiagnosticInfo message, DateTime receivedAt, string queueName);
    }
}
