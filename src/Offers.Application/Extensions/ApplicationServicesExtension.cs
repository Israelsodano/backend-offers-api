using Microsoft.Extensions.DependencyInjection;
using Offers.Application.Services;
using Offers.Domain.Services;

namespace Offers.Application.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUniversityService, UniversityService>();
            services.AddScoped<ICampusService, CampusService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IOfferService, OfferService>();
            return services;
        }
    }
}
