using Microsoft.EntityFrameworkCore;
using Order.Host.Data;
using Order.Host.Data.Entities;
using Order.Host.Models;
using Order.Host.Repositories.Interfaces;

namespace Order.Host.Repositories;

public class OrderItemRepository: IRepository<OrderItem, OrderItemModel>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<OrderItemRepository> _logger;

    public OrderItemRepository(ApplicationDbContext dbContext,
        ILogger<OrderItemRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<List<OrderItem>> GetAllItems()
    {
        List<OrderItem> orderItems = await _dbContext.OrderItems.ToListAsync();
        _logger.LogDebug($"found {orderItems.Count} order items");
        return orderItems;
    }

    public async Task<OrderItem> GetItemById(int id)
    {
        var orderItem = await _dbContext.OrderItems.FindAsync(id);
        if (orderItem == null)
        {
            _logger.LogDebug($"order item with id: {id} does not exist");
            throw new Exception($"Order item with ID: {id} does not exist");
        }
        _logger.LogDebug($"found order item: {orderItem.ToString()}");
        return orderItem;
    }

    public async Task<int?> AddItem(OrderItemModel item)
    {
        var order = await _dbContext.Orders.FindAsync(item.CustomerOrderId);
        if (order == null)
        {
            _logger.LogDebug($"order with id: {item.CustomerOrderId} does not exist");
            throw new Exception($"Order with ID: {item.CustomerOrderId} does not exist");
        }
        var orderItem = await _dbContext.OrderItems.AddAsync(new(){CustomerOrderId = order.Id, Amount = item.Amount, Subtotal = item.Subtotal});
        _logger.LogDebug($"new order item was added with id: {orderItem.Entity.Id}");
        await _dbContext.SaveChangesAsync();
        return orderItem.Entity.Id;
    }

    public async Task<OrderItem> UpdateItem(OrderItemModel item)
    {
        var orderItem = await GetItemById(item.Id);
        var order = await _dbContext.Orders.FindAsync(item.CustomerOrderId);
        if (order == null)
        {
            _logger.LogDebug($"order with id: {item.CustomerOrderId} does not exist");
            throw new Exception($"Order with ID: {item.CustomerOrderId} does not exist");
        }

        orderItem.CustomerOrder = order;
        orderItem.CustomerOrderId = order.Id;
        orderItem.Amount = item.Amount;
        orderItem.Subtotal = item.Subtotal;
        orderItem = _dbContext.OrderItems.Update(orderItem).Entity;
        await _dbContext.SaveChangesAsync();
        _logger.LogDebug($"order item with id: {orderItem.Id} was updated");
        return orderItem;
    }

    public async Task<OrderItem> RemoveItem(int id)
    {
        var orderItem = await GetItemById(id);
        _dbContext.OrderItems.Remove(orderItem);
        await _dbContext.SaveChangesAsync();
        _logger.LogDebug($"order item with id: {id} was removed");
        return orderItem;
    }
}