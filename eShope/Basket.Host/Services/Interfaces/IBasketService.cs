using Basket.Host.Models;

namespace Basket.Host.Services.Interfaces;

public interface IBasketService
{
    Task AddItemAsync(string userId, string data);
    Task<TestGetResponse> GetItemsAsync(string userId);
}