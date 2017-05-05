namespace PowerBI.Jobs.DocumentationFeedback.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using Context;

    public sealed class Configuration : DbMigrationsConfiguration<DocumentationFeedbackDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
        }
    }
}
