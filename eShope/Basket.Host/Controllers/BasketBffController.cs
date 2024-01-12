using System.Net;
using System.Security.Claims;
using Basket.Host.Models;
using Basket.Host.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = "basket.basketitem")]
[Route("api/v1/[controller]/")]
public class BasketBffController : ControllerBase
{
    private readonly ILogger<BasketBffController> _logger;
    private readonly IBasketService _basketService;

    public BasketBffController(
        ILogger<BasketBffController> logger,
        IBasketService basketService)
    {
        _logger = logger;
        _basketService = basketService;
    }
    
    [HttpPost("item")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> TestAdd(int itemId)
    {
        var basketId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _basketService.AddItem(basketId!, itemId);
        return Ok();
    }

    [HttpGet("items")]
    [ProducesResponseType(typeof(TestGetResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> TestGet()
    {
        var basketId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var response = await _basketService.GetItems(basketId!);
        return Ok(response);
    }
}