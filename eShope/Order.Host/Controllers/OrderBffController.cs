using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Host.Models;
using Order.Host.Services.interfaces;

namespace Order.Host.Controllers;

[ApiController]
[Authorize(Policy = "order.orderitem")]
[Route("order-bff")]
public class OrderBffController: ControllerBase
{
    private readonly ILogger<OrderBffController> _logger;
    private readonly IOrderBffService _service;

    public OrderBffController(ILogger<OrderBffController> logger,
        IOrderBffService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder(List<ItemModel> items)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = User.FindFirst(ClaimTypes.Name)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        int orderId = await _service.makeOrder(items, new() { Id = userId, Email = userEmail, Name = userName });

        return Ok(orderId);
    } 
    


}