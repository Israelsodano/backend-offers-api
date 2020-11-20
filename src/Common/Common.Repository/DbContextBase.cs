using Microsoft.EntityFrameworkCore;

namespace Common.Repository
{
    public abstract class DbContextBase<TContext> : DbContext where TContext : DbContextBase<TContext>
    {
        public DbContextBase(DbContextOptions<TContext> options) : base(options)
        { }

        internal virtual DbSet<EntityEvent> EntityEvents { get; set; }
    }
}
