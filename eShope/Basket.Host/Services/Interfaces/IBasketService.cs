using Basket.Host.Models;

namespace Basket.Host.Services.Interfaces;

public interface IBasketService
{
    Task AddItem(string userId, int itemId);
    Task<IEnumerable<int>> GetItems(string userId);
}