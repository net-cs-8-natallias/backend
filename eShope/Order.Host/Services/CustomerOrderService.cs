using Order.Host.Data.Entities;
using Order.Host.Models;
using Order.Host.Repositories.Interfaces;
using Order.Host.Services.interfaces;

namespace Order.Host.Services;

public class CustomerOrderService: IOrderService<CustomerOrder, CustomerOrderModel>
{
    private readonly ILogger<CustomerOrderService> _logger;
    private readonly IRepository<CustomerOrder, CustomerOrderModel> _repository;

    public CustomerOrderService(ILogger<CustomerOrderService> logger, 
        IRepository<CustomerOrder, CustomerOrderModel> repository)
    {
        _logger = logger;
        _repository = repository;
    }
    public async Task<List<CustomerOrder>> GetAllItems()
    {
        return await _repository.GetAllItems();
    }

    public async Task<CustomerOrder> GetItemById(int id)
    {
        return await _repository.GetItemById(id);
    }

    public async Task<int?> AddItem(CustomerOrderModel item)
    {
        return await _repository.AddItem(item);
    }

    public async Task<CustomerOrder> UpdateItem(CustomerOrderModel item)
    {
        return await _repository.UpdateItem(item);
    }

    public async Task<CustomerOrder> RemoveItem(int id)
    {
        return await _repository.RemoveItem(id);
    }
}