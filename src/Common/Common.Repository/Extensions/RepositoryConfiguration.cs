using System;
using System.Collections.Generic;
using Common.Repository.Application;
using Microsoft.EntityFrameworkCore.Internal;

namespace Common.Repository.Extensions
{
    internal class RepositoryConfiguration : IRepositoryConfiguration
    {
        private readonly IList<Type> _contexts;
        private readonly IList<KeyValuePair<Type, Type>> _units;
        public RepositoryConfiguration()
        {
            _contexts = new List<Type>();
            _units = new List<KeyValuePair<Type, Type>>();
        }

        public void AddUnit<TEntity, TContext>() where TEntity : EntityBase where TContext : DbContextBase<TContext>
        {
            if(!_contexts.Contains(typeof(TContext)))
                _contexts.Add(typeof(TContext));
            
            _units.Add(new KeyValuePair<Type, Type>(typeof(IUnitOfWork<TEntity>), typeof(UnityOfWork<TEntity, TContext>)));
        }

        internal IEnumerable<Type> GetContexts() => _contexts;
        internal IEnumerable<KeyValuePair<Type, Type>> GetUnits() => _units;
    }
}
