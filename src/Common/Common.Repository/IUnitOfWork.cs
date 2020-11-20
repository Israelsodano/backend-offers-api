using System;
using System.Threading.Tasks;

namespace Common.Repository
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task CommitAsync();
        internal Task BeginTransactionAsync();
        internal Task CommitTransactionAsync();
        internal Task RollBackTransactionAsync();
    }

    public interface IUnitOfWork<TEntity> : IUnitOfWork where TEntity : EntityBase
    {
        IRepository<TEntity> Repository { get; }
    }
}
