using System.Runtime.Serialization;
using Common.Repository;
using Common.Swagger.Extensions;
using Common.Versioning.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using Offers.Application.Extensions;

namespace Offers.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson(cfg => { 
                        cfg.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                        cfg.SerializerSettings.Converters.Add(new StringEnumConverter()); 
                    });

            services.AddVersioning();
            services.AddSwaggerDocumentation<Startup>();
            services.AddValidators();
            services.AddApplicationServices();
            
            services.ConfigureMediatR();
            services.ConfigureRepository(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider versionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseVersionedSwagger(versionDescriptionProvider);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            await ContextsMigrationManager.MigrateContextsAsync(app.ApplicationServices);
        }
    }
}
