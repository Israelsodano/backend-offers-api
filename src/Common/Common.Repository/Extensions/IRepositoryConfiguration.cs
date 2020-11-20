namespace Common.Repository.Extensions
{
    public interface IRepositoryConfiguration
    {
        void AddUnit<TEntity, TContext>() where TEntity : EntityBase where TContext : DbContextBase<TContext>;
    }
}
