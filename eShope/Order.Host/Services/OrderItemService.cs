using Order.Host.Data.Entities;
using Order.Host.Models;
using Order.Host.Repositories.Interfaces;
using Order.Host.Services.interfaces;

namespace Order.Host.Services;

public class OrderItemService: IOrderService<OrderItem, OrderItemModel>
{
    private readonly ILogger<OrderItemService> _logger;
    private readonly IRepository<OrderItem, OrderItemModel> _repository;

    public OrderItemService(ILogger<OrderItemService> logger, 
        IRepository<OrderItem, OrderItemModel> repository)
    {
        _logger = logger;
        _repository = repository;
    }
    public async Task<List<OrderItem>> GetAllItems()
    {
        return await _repository.GetAllItems();
    }

    public async Task<OrderItem> GetItemById(int id)
    {
        return await _repository.GetItemById(id);
    }

    public async Task<int?> AddItem(OrderItemModel item)
    {
        return await _repository.AddItem(item);
    }

    public async Task<OrderItem> UpdateItem(OrderItemModel item)
    {
        return await _repository.UpdateItem(item);
    }

    public async Task<OrderItem> RemoveItem(int id)
    {
        return await _repository.RemoveItem(id);
    }
}