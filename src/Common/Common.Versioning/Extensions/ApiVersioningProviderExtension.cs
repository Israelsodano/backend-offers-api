using Microsoft.Extensions.DependencyInjection;

namespace Common.Versioning.Extensions
{
    public static class ApiVersioningProviderExtension
    {
        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddApiVersioning(options => { options.ReportApiVersions = true; });
            return services;
        }
    }
}
