using System.Globalization;
using Infrastructure.RateLimit.models;
using Infrastructure.RateLimit.services.interfaces;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Infrastructure.RateLimit.services;

public class MiddlewareService
{
    private readonly RequestDelegate _next;
    private readonly IDatabase _redis;
    private readonly ILogger<MiddlewareService> _logger;
    private readonly IJsonSerializer _jsonSerializer;

    public MiddlewareService(RequestDelegate next,
        IConnectionMultiplexer redis, 
        IJsonSerializer jsonSerializer,
        ILogger<MiddlewareService> logger)
    {
        _next = next;
        _redis = redis.GetDatabase();
        _logger = logger;
        _jsonSerializer = jsonSerializer;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation($"*** Invoke async");
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        var endpointUrl = context.Request.Path.ToString();
        var key = new KeyModel() { Ip = ipAddress, Endpoint = endpointUrl };
        var serializedKey = _jsonSerializer.Serialize(key);
        
        var requestCount = await _redis.StringIncrementAsync(serializedKey);
        
        if (requestCount > 10)
        {
            context.Response.StatusCode = 429;
            await context.Response.WriteAsync("Too Many Requests.");
            return;
        }
        await _next(context);
    }
}