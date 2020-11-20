using Common.Repository;
using Microsoft.EntityFrameworkCore;
using Offers.Domain.Entities;

namespace Offers.Application.Contexts
{
    public class OffersContext : DbContextBase<OffersContext>
    {
        public OffersContext(DbContextOptions<OffersContext> options) : base(options)
        { }

        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<University> Universities { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Campus> Campus { get; set; }
    }
}
