using System;
using System.Threading.Tasks;
using Common.Repository.Application.Interfaces;

namespace Common.Repository
{
    public class ContextsMigrationManager
    {
        public static Task MigrateContextsAsync(IServiceProvider serviceProvider)
        {
            var orchestrator = (IContextsMigrationOrchestrator)serviceProvider.GetService(typeof(IContextsMigrationOrchestrator));
            return orchestrator.MigrateContexts(serviceProvider);
        }
    }
}
