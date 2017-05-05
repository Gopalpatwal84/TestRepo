namespace PowerBI.Jobs.DocumentationFeedback.Data.Context
{
    using PowerBI.Data;
    using PowerBI.Jobs.DocumentationFeedback.Data.Migrations;

    public class DocumentationFeedbackDbConfiguration : AzureDbConfiguration<DocumentationFeedbackDbContext, Configuration>
    {
    }
}
