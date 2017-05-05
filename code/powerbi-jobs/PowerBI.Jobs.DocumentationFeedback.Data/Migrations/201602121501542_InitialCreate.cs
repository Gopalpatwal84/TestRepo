namespace PowerBI.Jobs.DocumentationFeedback.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            this.CreateTable(
                "dbo.Feedback",
                c => new
                    {
                        FeedbackId = c.Guid(nullable: false),
                        ArticleId = c.String(nullable: false),
                        Culture = c.String(nullable: false, maxLength: 5),
                        Hash = c.String(),
                        GitHubSource = c.String(),
                        ArticleLastModified = c.DateTime(),
                        Service = c.String(),
                        FeedbackTimestamp = c.DateTime(nullable: false),
                        Url = c.String(nullable: false),
                        Helpful = c.Boolean(nullable: false),
                        IpAddress = c.String(),
                        AdditionalIpAddresses = c.String(),
                        Comment = c.String(),
                        ControlVersion = c.String(),
                        ExperimentId = c.String(),
                        VariationId = c.String(),
                    })
                .PrimaryKey(t => t.FeedbackId);
        }
        
        public override void Down()
        {
            this.DropTable("dbo.Feedback");
        }
    }
}
