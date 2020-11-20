using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Repository.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Common.Repository.Application
{
    internal class ContextsMigrationOrchestrator : IContextsMigrationOrchestrator
    {
        private IEnumerable<Type> _contexts;

        public ContextsMigrationOrchestrator(IEnumerable<Type> contexts) => _contexts = contexts;
        public Task MigrateContexts(IServiceProvider serviceProvider)
        {
            var tasks = _contexts.Select(x => Task.Run(() => {
                var context = (DbContext)serviceProvider.GetService(x);
                
                if(!context.Database.CanConnect());
                    context.Database.EnsureCreated();
            }));

            return Task.WhenAll(tasks);
        }
    }
}
