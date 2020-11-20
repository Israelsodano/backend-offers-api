using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Offers.Application.Pipelines
{
    public class LoggerPipeLineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    {
        private readonly ILogger _logger;
        public LoggerPipeLineBehavior(ILogger<LoggerPipeLineBehavior<TRequest, TResponse>> logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, 
                                      CancellationToken cancellationToken, 
                                      RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation($"Handling like: {typeof(TRequest).Name}. request: {JsonConvert.SerializeObject(request)}");
            var response = await next();
            _logger.LogInformation($"Handling like: {typeof(TResponse).Name}. response: {JsonConvert.SerializeObject(response)}");

            return response;
        }
    }
}
