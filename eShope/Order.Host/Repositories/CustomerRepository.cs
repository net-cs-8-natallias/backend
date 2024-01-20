using Microsoft.EntityFrameworkCore;
using Order.Host.Data;
using Order.Host.Data.Entities;
using Order.Host.Models;
using Order.Host.Repositories.Interfaces;

namespace Order.Host.Repositories;

public class CustomerRepository: ICustomerRepository<Customer, CustomerModel>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CustomerRepository> _logger;

    public CustomerRepository(ApplicationDbContext dbContext,
        ILogger<CustomerRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task<List<Customer>> GetAllItems()
    {
        List<Customer> customers = await _dbContext.Customers.ToListAsync();
        _logger.LogDebug($"found {customers.Count} customers");
        return customers;
    }

    public async Task<Customer> GetItemById(string id)
    {
        var customer = await _dbContext.Customers.FindAsync(id);
        if (customer == null)
        {
            _logger.LogDebug($"customer with id: {id} does not exist");
            throw new Exception($"Customer with ID: {id} does not exist");
        }
        _logger.LogDebug($"found customer: {customer.ToString()}");
        return customer;
    }

    public async Task<string?> AddItem(CustomerModel item)
    {
        var customer = await _dbContext.Customers.AddAsync( new() {Id = item.Id, Name = item.Name, Email = item.Email });
        _logger.LogDebug($"new customer was added with id: {customer.Entity.Id}");
        await _dbContext.SaveChangesAsync();
        return customer.Entity.Id;
    }

    public async Task<Customer> UpdateItem(CustomerModel item)
    {
        var customer = await GetItemById(item.Id);
        customer.Email = item.Email;
        customer.Name = item.Name;
        customer = _dbContext.Customers.Update(customer).Entity;
        await _dbContext.SaveChangesAsync();
        _logger.LogDebug($"customer with id: {customer.Id} was updated");
        return customer;
    }

    public async Task<Customer> RemoveItem(string id)
    {
        var customer = await GetItemById(id);
        _dbContext.Customers.Remove(customer);
        await _dbContext.SaveChangesAsync();
        _logger.LogDebug($"customer with id: {id} was removed");
        return customer;
    }
}