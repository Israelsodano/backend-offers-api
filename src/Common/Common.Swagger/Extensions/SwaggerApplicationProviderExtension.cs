using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;

namespace Common.Swagger.Extensions
{
    public static class SwaggerApplicationProviderExtension
    {
        public static IServiceCollection AddSwaggerDocumentation<T>(this IServiceCollection services) where T : class
        {
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerExamplesFromAssemblies(typeof(T).Assembly);
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            var applicationName = typeof(T).Assembly.GetName().Name.Replace(".", " ");

            services.AddSwaggerGen(options => 
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo 
                    { 
                        Title = applicationName,
                        Version = description.ApiVersion.ToString()
                    });
                }
            });

            return services;
        }

        public static IApplicationBuilder UseVersionedSwagger(this IApplicationBuilder builder, IApiVersionDescriptionProvider versionProvider)
        {
            builder.UseSwagger(o => o.RouteTemplate = "swagger/{documentName}/swagger.json");
            builder.UseSwaggerUI(options =>
            {

                foreach (var description in versionProvider.ApiVersionDescriptions)
                {
                    options.RoutePrefix = "swagger";
                    options.SwaggerEndpoint($"../swagger/{description.GroupName}/swagger.json", description.GroupName);
                }
            });

            return builder;
        }
    }
}
