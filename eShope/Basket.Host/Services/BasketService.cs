using Basket.Host.Models;
using Basket.Host.Services.Interfaces;

namespace Basket.Host.Services;

public class BasketService : IBasketService
{
    private readonly ILogger<BasketService> _logger;
    private readonly Dictionary<string, List<int>> _basket;

    public BasketService(ILogger<BasketService> logger)
    {
        _basket = new Dictionary<string, List<int>>();
        _logger = logger;
    }
    
    public async Task AddItem(string userId, int itemId)
    {
        if (_basket.ContainsKey(userId))
        {
            _basket[userId].Add(itemId);
        }
        else
        {
            _basket.Add(userId, new List<int>(){itemId});
        }
    }

    public async Task<IEnumerable<int>> GetItems(string userId)
    {
        return _basket[userId];
    }
}