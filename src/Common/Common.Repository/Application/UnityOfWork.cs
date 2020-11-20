using System;
using System.Threading.Tasks;
using Common.Repository.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Common.Repository.Application
{
    internal sealed class UnityOfWork<TEntity, TContext> : IUnitOfWork<TEntity> 
        where TEntity : EntityBase
        where TContext : DbContextBase<TContext>
    {
        private readonly TContext _dbContext;
        private readonly IEventManeger _eventManeger;
        private IDbContextTransaction _transaction;

        public UnityOfWork(TContext dbContext, IEventManeger eventManeger, IUnitsTransactionOrchestrator transactionOrchestrator)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _eventManeger = eventManeger ?? throw new ArgumentNullException(nameof(eventManeger));
            Repository = new Repository<TEntity>(dbContext.Set<TEntity>(), eventManeger);

            transactionOrchestrator.AddUnit(this);
        }

        public IRepository<TEntity> Repository { get; private set; }

        public async Task CommitAsync()
        {
            await _dbContext.AddRangeAsync(_eventManeger.GetCelebratedEvents());
            await _dbContext.SaveChangesAsync();
            _eventManeger.ClearEvents();
        }

        async Task IUnitOfWork.BeginTransactionAsync() => _transaction = await _dbContext.Database.BeginTransactionAsync();

        Task IUnitOfWork.CommitTransactionAsync() => _transaction.CommitAsync();

        Task IUnitOfWork.RollBackTransactionAsync() => _transaction.RollbackAsync();

        ValueTask IAsyncDisposable.DisposeAsync()
        {
            _dbContext.Dispose();
            _eventManeger.Dispose();
            return new ValueTask(Task.CompletedTask);
        }
    }
}
