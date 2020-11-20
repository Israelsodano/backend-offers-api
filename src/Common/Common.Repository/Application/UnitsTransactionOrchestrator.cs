using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Repository.Application
{
    internal sealed class UnitsTransactionOrchestrator : IUnitsTransactionOrchestrator
    {
        private IList<IUnitOfWork> _units;
        public bool isTransaction { get; private set; }
        public UnitsTransactionOrchestrator() => _units = new  List<IUnitOfWork>();
        public Task BeginTransactionsAsync() => Task.Run(() => isTransaction = true);
        
        public Task CommitTransactionsAsync() => Task.WhenAll(_units.Select(x => x.CommitTransactionAsync()));     
        public Task RollBackTransactionsAsync() => Task.WhenAll(_units.Select(x => x.RollBackTransactionAsync()));
        void IUnitsTransactionOrchestrator.AddUnit(IUnitOfWork unit)
        {
            if (isTransaction)
               Task.WaitAll(unit.BeginTransactionAsync());
            
            _units.Add(unit);
        }

        public void Dispose() => Task.WhenAll(_units.Select(x => x.DisposeAsync().AsTask())).Wait();
    }
}
