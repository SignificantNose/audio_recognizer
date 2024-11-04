using System;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Covers.Interceptors;

public class LoggingInterceptor : Interceptor
{
    private readonly ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        _logger.LogInformation(
            "START: Handling method {Method} of request {request}. Request hash: {HashCode}", 
            context.Method,
            request,
            request.GetHashCode()
        );
        
        var response = await continuation(request, context);

        _logger.LogInformation(
            "FINISHED: Handled method {method} of request with hash {HashCode}. Response = {response}.",
            context.Method,
            request.GetHashCode(),
            response
        );

        return response;
    }

}
