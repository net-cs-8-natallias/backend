using Microsoft.AspNetCore.Mvc;
using Order.Host.Data.Entities;
using Order.Host.Models;
using Order.Host.Services.interfaces;

namespace Order.Host.Controllers;

[ApiController]
[Route("customer-order")]
public class CustomerOrderController: ControllerBase
{
    private readonly ILogger<CustomerOrderController> _logger;
    private readonly IOrderService<CustomerOrder, CustomerOrderModel> _service;

    public CustomerOrderController(ILogger<CustomerOrderController> logger,
        IOrderService<CustomerOrder, CustomerOrderModel> service)
    {
        _logger = logger;
        _service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult> Orders()
    {
        var orders = await _service.GetAllItems();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Order(int id)
    {
        var order = await _service.GetItemById(id);
        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult> Add(CustomerOrderModel customerOrder)
    {
        var orderId = await _service.AddItem(customerOrder);
        return Ok(orderId);
    }
    
    [HttpPut]
    public async Task<ActionResult> Update(CustomerOrderModel customerOrder)
    {
        var order = await _service.UpdateItem(customerOrder);
        return Ok(order);
    }
    
    [HttpDelete]
    public async Task<ActionResult> Delete(int id)
    {
        var order = await _service.RemoveItem(id);
        return Ok(order);
    }
}