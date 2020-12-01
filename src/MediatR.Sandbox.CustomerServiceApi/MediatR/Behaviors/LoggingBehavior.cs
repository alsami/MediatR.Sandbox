using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MediatR.Sandbox.CustomerServiceApi.MediatR.Behaviors
{
    // ReSharper disable once UnusedType.Global
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation("Executing request {request}", typeof(TRequest).Name);
            var sw = Stopwatch.StartNew();
            var response = await next();
            sw.Stop();
            _logger.LogInformation("Executed request {request} in {ms} milli-seconds.", typeof(TRequest).Name, sw.ElapsedMilliseconds);
            return response;
        }
    }
}