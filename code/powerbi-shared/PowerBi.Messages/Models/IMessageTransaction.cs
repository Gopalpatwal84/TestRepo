namespace PowerBI.Messages.Models
{
    using System;

    public interface IMessageTransaction
    {
        Guid TransactionId { get; set; }

        DateTime TransactionStarted { get; set; }
    }
}