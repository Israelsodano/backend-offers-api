using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.Repository.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Common.Repository.Application
{
    internal struct Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        private readonly DbSet<TEntity> _dbset;
        
        private readonly IEventManeger _eventManeger;
        internal Repository(DbSet<TEntity> dbset, IEventManeger eventManeger)
        {
            _dbset = dbset ?? throw new ArgumentNullException(nameof(dbset));
            _eventManeger = eventManeger ?? throw new ArgumentNullException(nameof(eventManeger));
        }

        public async Task AddAsync(TEntity entity)
        {
            CelebrateEvent(EntityState.Added, entity);
            await _dbset.AddAsync(entity);
        }

        public Task AddAsync(IEnumerable<TEntity> entities)
        {
            CelebrateEvent(EntityState.Added, entities);
            return _dbset.AddRangeAsync(entities);
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            var entities = await GetAll(expression);
            CelebrateEvent(EntityState.Deleted, entities);
            _dbset.RemoveRange(entities);
        }

        public Task UpdateAsync(TEntity entity) 
        {
            CelebrateEvent(EntityState.Modified, entity);
            _dbset.Update(entity);

            return Task.CompletedTask;
        } 

        public Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            CelebrateEvent(EntityState.Modified, entities);
            _dbset.UpdateRange(entities);

            return Task.CompletedTask;
        }

        public Task<IQueryable<TEntity>> GetAll() => Task.FromResult<IQueryable<TEntity>>(_dbset);
        public Task<IQueryable<TEntity>> GetAll(Expression<Func<TEntity, bool>> expression) => Task.FromResult(_dbset.Where(expression));
        public Task<TEntity> GetFirst(Expression<Func<TEntity, bool>> expression) => _dbset.FirstOrDefaultAsync(expression);
        private void CelebrateEvent(EntityState state, TEntity entity) => _eventManeger.CelebrateEvent(state, entity);
        private void CelebrateEvent(EntityState state, IEnumerable<TEntity> entities) => _eventManeger.CelebrateEvent(state, entities);
    }
}
