namespace Basket.Host.Services.Interfaces;

public interface ICacheService
{
    Task AddOrUpdateAsync<T>(string userId, T data);
    
    Task<T>GetAsync<T>(string userId);
}