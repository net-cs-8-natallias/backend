using Infrastructure.RateLimit.models;
using Infrastructure.RateLimit.services.interfaces;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Infrastructure.RateLimit.services;

public class FilterService: IFilterService
{
    private readonly ILogger<FilterService> _logger;
    private readonly IDatabase _redis;
    private readonly IJsonSerializer _jsonSerializer;
    
    public FilterService(ILogger<FilterService> logger, 
        IConnectionMultiplexer redis, IJsonSerializer jsonSerializer)
    {
        _logger = logger;
        _redis = redis.GetDatabase();
        _jsonSerializer = jsonSerializer;
    }
    
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context, 
        ActionExecutionDelegate next)
    {
        _logger.LogInformation($"*** Execution action");
        var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
        var endpointUrl = context.HttpContext.Request.Path.ToString();
        var key = new KeyModel() { Ip = ipAddress, Endpoint = endpointUrl };
        var serializedKey = _jsonSerializer.Serialize(key);
        
        var requestCount = await _redis.StringIncrementAsync(serializedKey);
        if (requestCount > 10)
        {
            context.Result = new ContentResult
            {
                StatusCode = 429,
                Content = "Too Many Requests."
            };
        }

        await next();
    }

}