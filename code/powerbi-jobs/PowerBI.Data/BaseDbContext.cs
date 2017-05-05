namespace PowerBI.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public abstract class BaseDbContext : DbContext, IDbContext
    {
        protected BaseDbContext()
            : base(new PowerBIUserDbConnectionString())
        {
        }

        public void Initialize()
        {
            this.Database.Initialize(false);
        }

        IDbSet<TEntity> IDbContext.Set<TEntity>()
        {
            return this.Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
