using System;
using System.Threading.Tasks;

namespace Common.Repository.Application.Interfaces
{
    internal interface IContextsMigrationOrchestrator
    { 
        Task MigrateContexts(IServiceProvider serviceProvider);
    }
}
