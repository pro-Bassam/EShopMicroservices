using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : notnull, IRequest<TResponse> 
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await next();
        stopwatch.Stop();
        var elapsedSeconds = stopwatch.ElapsedMilliseconds / 1000.0;


        // if more than 3 sec warning
        if (elapsedSeconds > 3)
        {
            logger.LogWarning("[PERFORMANCE] The request {Request} took {elapsedSeconds}",
                typeof(TRequest).Name, elapsedSeconds);
        }

        return response;
    }
}