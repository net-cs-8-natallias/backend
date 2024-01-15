using System.Net;
using System.Security.Claims;
using Basket.Host.Models;
using Basket.Host.Services;
using Basket.Host.Services.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = "basket.basketitem")]
[Route(ComponentsDefaults.DefaultRoute)]
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
    [Filter]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> TestAdd(TestAddRequest data)
    {
        var basketId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _basketService.AddItemAsync(basketId!, data.Data);
        return Ok();
    }

    [HttpGet("items")]
    [Filter]
    [ProducesResponseType(typeof(TestGetResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> TestGet()
    {
        var basketId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var response = await _basketService.GetItemsAsync(basketId!);
        return Ok(response);
    }
}