using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gateway.Authentication;

public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private readonly IConfiguration _config;

    public ApiKeyAuthFilter(IConfiguration config)
    {
        _config = config;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if(!context.HttpContext.Request.Headers.TryGetValue(
            AuthConstants.ApiKeyHeaderName, 
            out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API key is missing");
        }
        
        var validApiKey = _config.GetValue<string>(AuthConstants.ApiKeySectionName);
        if(!extractedApiKey.Equals(validApiKey)){
            context.Result = new UnauthorizedObjectResult("API key is invalid");
        }
    }
}
