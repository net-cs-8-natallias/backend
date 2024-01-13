using System.Net;
using Catalog.Host.Configurations;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Services.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Catalog.Host.Controllers;

[Authorize(Policy = "ApiScope")]
[ApiController]
[Route(ComponentsDefaults.DefaultRoute)]
public class CatalogBffController : ControllerBase
{
    private readonly IOptions<CatalogConfigurations> _config;
    private readonly ILogger<CatalogBffController> _logger;
    private readonly IBffService _service;

    public CatalogBffController(
        ILogger<CatalogBffController> logger,
        IBffService service,
        IOptions<CatalogConfigurations> config)
    {
        _logger = logger;
        _service = service;
        _config = config;
    }

    [HttpGet("items")]
    [ProducesResponseType(typeof(PaginatedItems<CatalogItem>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetItems(int pageSize, int pageIndex, int brand, int type)
    {
        _logger.LogInformation(
            $"*bff-controller* request to get items by page size: {pageSize}, page index: {pageIndex}");
        var catalogItems = await _service.GetItems(pageSize, pageIndex, brand, type);
        return Ok(catalogItems);
    }

    [HttpGet("brands")]
    public async Task<ActionResult> Brands()
    {
        _logger.LogInformation($"*{GetType().Name}* request to get all brands");
        var catalogBrands = await _service.GetBrands();
        return Ok(catalogBrands);
    }

    [HttpGet("types")]
    public async Task<ActionResult> Types()
    {
        _logger.LogInformation($"*{GetType().Name}* request to get all types");
        var catalogTypes = await _service.GetTypes();
        return Ok(catalogTypes);
    }

    [HttpGet("items/{id:int}")]
    public async Task<ActionResult> GetItem(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get item by id: {id}");
        var item = await _service.GetItem(id);
        return Ok(item);
    }

    [HttpGet("brands/{id}")]
    public async Task<ActionResult> GetBrand(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get item by id: {id}");
        var item = await _service.GetBrand(id);
        return Ok(item);
    }

    [HttpGet("types/{id}")]
    public async Task<ActionResult> GetType(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get item by id: {id}");
        var item = await _service.GetType(id);
        return Ok(item);
    }

    [HttpGet("items/type")]
    public async Task<ActionResult> GetByType([FromQuery] string type)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get items by id: {type}");
        var items = await _service.GetItemByType(type);
        return Ok(items);
    }

    [HttpGet("items/brand")]
    public async Task<ActionResult> GetByBrand([FromQuery] string brand)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get items by id: {brand}");
        var items = await _service.GetItemByBrand(brand);
        return Ok(items);
    }
}