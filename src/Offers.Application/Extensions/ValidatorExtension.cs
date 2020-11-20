using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Offers.Application.Validators;
using Offers.Domain.Entities;

namespace Offers.Application.Extensions
{
    public static class ValidatorExtension
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<University>, UniversityValidator>();
            services.AddTransient<IValidator<Campus>, CampusValidator>();
            services.AddTransient<IValidator<Course>, CourseValidator>();
            services.AddTransient<IValidator<Offer>, OfferValidator>();

            return services;
        }
    }
}
