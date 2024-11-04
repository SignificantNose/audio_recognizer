using System;

namespace Gateway.Authentication;

public class ApiKeyAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;

    public ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration config){
        _next = next;
        _config = config;
    }

    public async Task InvokeAsync(HttpContext context){
        if(!context.Request.Headers.TryGetValue(
            AuthConstants.ApiKeyHeaderName, 
            out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API key is missing");
            return;
        }
        
        var validApiKey = _config.GetValue<string>(AuthConstants.ApiKeySectionName);
        if(!extractedApiKey.Equals(validApiKey)){
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API key is invalid");
            return;
        }

        await _next(context);
    }
}
