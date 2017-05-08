namespace BusinessPlatform.Jobs.CommonModules.Instrumentation
{
    using System;
    using BusinessPlatform.Messages.Models;

    public interface IMessageInstrumentationService
    {
        void SaveOrUpdateInstrumentationInfo(IDiagnosticInfo message, DateTime receivedAt, string queueName);
    }
}
