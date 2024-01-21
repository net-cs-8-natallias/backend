using Microsoft.EntityFrameworkCore;
using Order.Host.Data;
using Order.Host.Data.Entities;
using Order.Host.Models;
using Order.Host.Repositories.Interfaces;

namespace Order.Host.Repositories;

public class CustomerOrderRepository: IRepository<CustomerOrder, CustomerOrderModel>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CustomerOrderRepository> _logger;

    public CustomerOrderRepository(ApplicationDbContext dbContext,
        ILogger<CustomerOrderRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task<List<CustomerOrder>> GetAllItems()
    {
        List<CustomerOrder> customerOrders = await _dbContext.Orders.ToListAsync();
        _logger.LogDebug($"found {customerOrders.Count} orders");
        return customerOrders;
    }

    public async Task<CustomerOrder> GetItemById(int id)
    {
        var customerOrder = await _dbContext.Orders.FindAsync(id);
        if (customerOrder == null)
        {
            _logger.LogDebug($"order with id: {id} does not exist");
            throw new Exception($"Order with ID: {id} does not exist");
        }
        _logger.LogDebug($"found order: {customerOrder.ToString()}");
        return customerOrder;
    }

    public async Task<int?> AddItem(CustomerOrderModel item)
    {
        var customer = await _dbContext.Customers.FindAsync(item.CustomerId);
        if (customer == null)
        {
            _logger.LogDebug($"customer with id: {item.CustomerId} does not exist");
            throw new Exception($"Customer with ID: {item.CustomerId} does not exist");
        }
        var customerOrder = await _dbContext.Orders.AddAsync(new (){CustomerId = customer.Id, Date = new DateTime(), TotalPrice = item.TotalPrice });
        _logger.LogDebug($"new order was added with id: {customerOrder.Entity.Id}");
        await _dbContext.SaveChangesAsync();
        return customerOrder.Entity.Id;
    }

    public async Task<CustomerOrder> UpdateItem(CustomerOrderModel item)
    {
        var customerOrder = await GetItemById(item.Id);
        var customer = await _dbContext.Customers.FindAsync(item.CustomerId);
        if (customer == null)
        {
            _logger.LogDebug($"customer with id: {item.CustomerId} does not exist");
            throw new Exception($"Customer with ID: {item.CustomerId} does not exist");
        }

        customerOrder.Customer = customer;
        customerOrder.CustomerId = customer.Id;
        customerOrder.Date = item.Date;
        customerOrder.TotalPrice = item.TotalPrice;
        customerOrder = _dbContext.Orders.Update(customerOrder).Entity;
        await _dbContext.SaveChangesAsync();
        _logger.LogDebug($"order with id: {customerOrder.Id} was updated");
        return customerOrder;
    }

    public async Task<CustomerOrder> RemoveItem(int id)
    {
        var customerOrder = await GetItemById(id);
        _dbContext.Orders.Remove(customerOrder);
        await _dbContext.SaveChangesAsync();
        _logger.LogDebug($"order with id: {id} was removed");
        return customerOrder;
    }
}