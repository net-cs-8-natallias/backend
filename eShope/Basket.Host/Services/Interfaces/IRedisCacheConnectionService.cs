using StackExchange.Redis;

namespace Basket.Host.Services.Interfaces;

public interface IRedisCacheConnectionService
{
    IConnectionMultiplexer Connection { get; }
}