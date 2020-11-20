using Common.Repository.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Offers.Application.Contexts;
using Offers.Domain.Entities;

namespace Offers.Application.Extensions
{
    public static class RepositoryExtension
    {
        public static IServiceCollection ConfigureRepository(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddSingleton(x => new DbContextOptionsBuilder<OffersContext>()
            //        .UseSqlServer(configuration.GetConnectionString("SqlServer")).Options);

            services.AddSingleton(x => new DbContextOptionsBuilder<OffersContext>()
                    .UseSqlite(configuration.GetConnectionString("Sqlite")).Options);

            services.AddRepositoryServices(cfg => { 
                cfg.AddUnit<University, OffersContext>();
                cfg.AddUnit<Campus, OffersContext>();
                cfg.AddUnit<Course, OffersContext>();
                cfg.AddUnit<Offer, OffersContext>();
            });

            return services;
        }
    }
}
