using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Order.Host.Data.Entities;
using Order.Host.Models;
using Order.Host.Services.interfaces;

namespace Order.Host.Controllers;


[ApiController]
[Route("customer")]
public class CustomerController: ControllerBase
{
    private readonly ILogger<CustomerController> _logger;
    private readonly IOrderService<Customer, CustomerModel> _service;

    public CustomerController(ILogger<CustomerController> logger,
        IOrderService<Customer, CustomerModel> service)
    {
        _logger = logger;
        _service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult> Customers()
    {
        var customers = await _service.GetAllItems();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Customer(int id)
    {
        var customer = await _service.GetItemById(id);
        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult> Add(CustomerModel customer)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        customer.Id = userId;
        var custumerId = await _service.AddItem(customer);
        return Ok(custumerId);
    }
    
    [HttpPut]
    public async Task<ActionResult> Update(CustomerModel customer)
    {
        var custumer = await _service.UpdateItem(customer);
        return Ok(custumer);
    }
    
    [HttpDelete]
    public async Task<ActionResult> Delete(int id)
    {
        var custumer = await _service.RemoveItem(id);
        return Ok(custumer);
    }
}