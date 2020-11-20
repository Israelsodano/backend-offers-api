using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Repository
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        Task<IQueryable<TEntity>> GetAll();
        Task<IQueryable<TEntity>> GetAll(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetFirst(Expression<Func<TEntity, bool>> expression);
        Task AddAsync(TEntity entity);
        Task AddAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task UpdateAsync(IEnumerable<TEntity> entities);
        Task DeleteAsync(Expression<Func<TEntity, bool>> expression);
    }
}
