using Basket.Host.Configuration;
using Basket.Host.Services.Interfaces;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Basket.Host.Services;

public class CacheService: ICacheService
{
    private readonly IRedisCacheConnectionService _redisCacheConnectionService;
    private readonly IOptions<RedisConfig> _config;
    private readonly ILogger<ICacheService> _logger;
    private readonly IJsonSerializer _jsonSerializer;
        
    public CacheService(IRedisCacheConnectionService redisCacheConnectionService, 
        IOptions<RedisConfig> config, ILogger<ICacheService> logger,
        IJsonSerializer jsonSerializer)
    {
        _redisCacheConnectionService = redisCacheConnectionService;
        _config = config;
        _logger = logger;
        _jsonSerializer = jsonSerializer;
    }
    public async Task AddOrUpdateAsync<T>(string userId, T data)
    {
        var redis = GetRedisDatabase();
        var serialized = _jsonSerializer.Serialize(data);
        if (await redis.StringSetAsync(userId, serialized, _config.Value.CacheTimeout))
        {
            _logger.LogInformation($"Value has been cached");
        }
        else
        {
            _logger.LogInformation($"Value has been updated");
        }
    }

    public async Task<T> GetAsync<T>(string userId)
    {
        var redis = GetRedisDatabase();
        var serialized = await redis.StringGetAsync(userId);
        return serialized.HasValue ? _jsonSerializer.Deserialize<T>(serialized.ToString()) : default(T)!;
    }

    private IDatabase GetRedisDatabase() => _redisCacheConnectionService.Connection.GetDatabase();
}