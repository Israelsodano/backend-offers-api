using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Common.Repository.Application
{
    internal sealed class UnitsTransactionOrchestrator : IUnitsTransactionOrchestrator
    {
        private IList<IUnitOfWork> _units;
        public bool IsInTransaction { get; private set; }
        private readonly IHostingEnvironment _environment;
        public UnitsTransactionOrchestrator(IHostingEnvironment environment)
        {
            _units = new List<IUnitOfWork>();
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public Task BeginTransactionsAsync() => Task.Run(() => {
            IsInTransaction = true;
            
            if(!_environment.IsEnvironment("Test"))
                Task.WhenAll(_units.Select(x => x.BeginTransactionAsync()));
        });

        public Task CommitTransactionsAsync() => _environment.IsEnvironment("Test") ? 
                                                    Task.CompletedTask :
                                                    Task.WhenAll(_units.Select(x => x.CommitTransactionAsync()));     
        public Task RollBackTransactionsAsync() => _environment.IsEnvironment("Test") ?
                                                    Task.CompletedTask : 
                                                    Task.WhenAll(_units.Select(x => x.RollBackTransactionAsync()));
        void IUnitsTransactionOrchestrator.AddUnit(IUnitOfWork unit)
        {
            if (IsInTransaction && !_environment.IsEnvironment("Test"))
               Task.WaitAll(unit.BeginTransactionAsync());
            
            _units.Add(unit);
        }

        public void Dispose() => Task.WhenAll(_units.Select(x => x.DisposeAsync().AsTask())).Wait();
    }
}
