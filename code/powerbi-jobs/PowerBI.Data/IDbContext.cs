namespace PowerBI.Data
{
    using System.Data.Entity;

    public interface IDbContext
    {
        void Initialize();

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();
    }
}
