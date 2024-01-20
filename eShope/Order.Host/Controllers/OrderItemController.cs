using Microsoft.AspNetCore.Mvc;
using Order.Host.Data.Entities;
using Order.Host.Models;
using Order.Host.Services.interfaces;

namespace Order.Host.Controllers;

[ApiController]
[Route("order-item")]
public class OrderItemController: ControllerBase
{
    private readonly ILogger<OrderItemController> _logger;
    private readonly IOrderService<OrderItem, OrderItemModel> _service;

    public OrderItemController(ILogger<OrderItemController> logger,
        IOrderService<OrderItem, OrderItemModel> service)
    {
        _logger = logger;
        _service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult> OrderItems()
    {
        var orderItems = await _service.GetAllItems();
        return Ok(orderItems);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> OrderItemModel(int id)
    {
        var orderItem = await _service.GetItemById(id);
        return Ok(orderItem);
    }

    [HttpPost]
    public async Task<ActionResult> Add(OrderItemModel OrderItem)
    {
        var orderItemId = await _service.AddItem(OrderItem);
        return Ok(orderItemId);
    }
    
    [HttpPut]
    public async Task<ActionResult> Update(OrderItemModel item)
    {
        var orderItem = await _service.UpdateItem(item);
        return Ok(orderItem);
    }
    
    [HttpDelete]
    public async Task<ActionResult> Delete(int id)
    {
        var orderItem = await _service.RemoveItem(id);
        return Ok(orderItem);
    }
}
