namespace PowerBI.Jobs.DocumentationFeedback.Data.Context
{
    using System.Data.Entity;
    using PowerBI.Data;
    using PowerBI.Jobs.DocumentationFeedback.Data.Models;

    public interface IDocumentationFeedbackContext : IDbContext
    {
        IDbSet<Feedback> Feedback { get; }
    }
}
