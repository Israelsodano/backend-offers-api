using System;
using System.Threading.Tasks;

namespace Common.Repository
{
    public interface IUnitsTransactionOrchestrator : IDisposable
    {
        Task BeginTransactionsAsync();

        Task CommitTransactionsAsync();

        Task RollBackTransactionsAsync();

        internal void AddUnit(IUnitOfWork unit);
    }
}
