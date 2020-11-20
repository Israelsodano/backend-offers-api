using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Offers.Application.Pipelines;

namespace Offers.Application.Extensions
{
    public static class MediatRExtension
    {
        public static IServiceCollection ConfigureMediatR(this IServiceCollection services)
        {
            services.AddMediatR(typeof(MediatRExtension).Assembly);

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggerPipeLineBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionPipeLineBehavior<,>));

            return services;
        }
    }
}
