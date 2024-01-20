using Order.Host.Data.Entities;
using Order.Host.Models;
using Order.Host.Repositories.Interfaces;
using Order.Host.Services.interfaces;

namespace Order.Host.Services;

public class CustomerService: IOrderService<Customer, CustomerModel>
{
    private readonly ILogger<CustomerService> _logger;
    private readonly ICustomerRepository<Customer, CustomerModel> _repository;

    public CustomerService(ILogger<CustomerService> logger, 
        ICustomerRepository<Customer, CustomerModel> repository)
    {
        _logger = logger;
        _repository = repository;
    }
    
    public async Task<List<Customer>> GetAllItems()
    {
        return await _repository.GetAllItems();
    }

    public async Task<Customer> GetItemById(int id)
    {
        return await _repository.GetItemById(id.ToString());
    }

    public async Task<int?> AddItem(CustomerModel item)
    {
        string? id = await _repository.AddItem(item);
        return int.Parse(id);
    }

    public async Task<Customer> UpdateItem(CustomerModel item)
    {
        return await _repository.UpdateItem(item);
    }

    public async Task<Customer> RemoveItem(int id)
    {
        return await _repository.RemoveItem(id.ToString());
    }
}