using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Repository;
using MediatR;

namespace Offers.Application.Pipelines
{
    class TransactionPipeLineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IUnitsTransactionOrchestrator _transactionOrchestrator;
        public TransactionPipeLineBehavior(IUnitsTransactionOrchestrator transactionOrchestrator) =>
            _transactionOrchestrator = transactionOrchestrator ?? throw new ArgumentNullException(nameof(transactionOrchestrator));

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                await _transactionOrchestrator.BeginTransactionsAsync();
                var response = await next();
                await _transactionOrchestrator.CommitTransactionsAsync();

                return response;
            }
            catch (Exception ex)
            {
                await _transactionOrchestrator.RollBackTransactionsAsync();
                throw ex;
            }
        }
    }
}
