using System;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Utils
{
    public class PagedEntities
    {
        public static IQueryable<TEntity> GetPagedEntities<TEntity>(int page, 
                                                                    int range,
                                                                    int count,
                                                                    bool desc,
                                                                    IQueryable<TEntity> entities,
                                                                    Expression<Func<TEntity, dynamic>> orderBy) where TEntity : class
        {
            if (count == 0)
                return null;

            int skip, take;
            skip = range * page;
            take = range == 0 ? 10 : range;

            skip = skip > count ? count : skip;
            take = count < skip + take ? count - skip : take;

            return desc is false ? 
                    entities.OrderBy(orderBy).Skip(skip).Take(take) : 
                    entities.OrderByDescending(orderBy).Skip(skip).Take(take);
        }
    }
}
