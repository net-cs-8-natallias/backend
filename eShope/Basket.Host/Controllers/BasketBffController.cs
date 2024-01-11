using System.Net;
using Basket.Host.Models;
using Basket.Host.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = "ApiScope")]
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
    
    [HttpPost("add")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> TestAdd(int itemId)
    {
        var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        await _basketService.AddItem(basketId!, itemId);
        return Ok();
    }

    [HttpGet("get")]
    [ProducesResponseType(typeof(TestGetResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> TestGet()
    {
        var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        var response = await _basketService.GetItems(basketId!);
        return Ok(response);
    }
}