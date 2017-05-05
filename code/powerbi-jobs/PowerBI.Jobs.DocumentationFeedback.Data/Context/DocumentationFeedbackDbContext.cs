namespace PowerBI.Jobs.DocumentationFeedback.Data.Context
{
    using System.Data.Entity;
    using PowerBI.Data;
    using PowerBI.Jobs.DocumentationFeedback.Data.Models;

    public class DocumentationFeedbackDbContext : BaseDbContext, IDocumentationFeedbackContext
    {
        public IDbSet<Feedback> Feedback
        {
            get
            {
                return this.Set<Feedback>();
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Feedback>();
        }
    }
}
