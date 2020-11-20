using System;
using System.Collections.Generic;
using Common.Repository.Application;
using Common.Repository.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Repository.Extensions
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services, Action<IRepositoryConfiguration> action)
        {
            var configuration = new RepositoryConfiguration();
            action(configuration);

            AddContexts(configuration.GetContexts(), services);
            AddUnitsOfWork(configuration.GetUnits(), services);
        
            services.AddSingleton<IEventLabels, EventLabels>();
            services.AddScoped<IEventManeger, EventManeger>();
            services.AddScoped<IUnitsTransactionOrchestrator, UnitsTransactionOrchestrator>();

            return services;
        }

        private static void AddContexts(IEnumerable<Type> contexts, IServiceCollection services)
        {
            services.AddSingleton<IContextsMigrationOrchestrator>(x => new ContextsMigrationOrchestrator(contexts));

            foreach (var context in contexts)
                services.AddTransient(context);
        }

        private static void AddUnitsOfWork(IEnumerable<KeyValuePair<Type, Type>> units, IServiceCollection services)
        {
            
            foreach (var unit in units)
                services.AddScoped(unit.Key, unit.Value);
        }
    }
}
