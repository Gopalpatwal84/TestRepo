namespace PowerBI.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.SqlServer;
    using System.Linq;

    public class AzureDbConfiguration<TDbContext, TMigrationConfiguration> : DbConfiguration
    where TDbContext : DbContext
    where TMigrationConfiguration : DbMigrationsConfiguration<TDbContext>, new()
    {
        private const string ProviderName = "System.Data.SqlClient";

        protected AzureDbConfiguration()
        {
            this.SetExecutionStrategy(ProviderName, () => new SqlAzureExecutionStrategy());
            this.SetDatabaseInitializer(new DbMigratorInitializer<TDbContext, TMigrationConfiguration>());
        }
    }

    public class DbMigratorInitializer<TContext, TConfiguration> : IDatabaseInitializer<TContext>
        where TContext : DbContext
        where TConfiguration : DbMigrationsConfiguration<TContext>, new()
    {
        public void InitializeDatabase(TContext context)
        {
            var migrator = new DbMigrator(new TConfiguration());

            var pending = migrator.GetPendingMigrations();
            if (pending.Any())
            {
                migrator.Update();
            }
        }
    }
}