using System;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Metadata.Interceptors;

public class ErrorInterceptor : Interceptor
{
    private readonly ILogger<ErrorInterceptor> _logger;

    public ErrorInterceptor(ILogger<ErrorInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try{
            return await continuation(request, context);
        }
        catch(RpcException ex){
            _logger.LogError("Error handling method {Method}. Error = {Message}", 
                context.Method,
                ex.Message);
            throw;
        }
        catch(Exception ex){
            _logger.LogError("Unexpected error occurred when handling method {Method}. Error = {Message}", 
                context.Method,
                ex.Message);
            throw;
        }
    }
}
