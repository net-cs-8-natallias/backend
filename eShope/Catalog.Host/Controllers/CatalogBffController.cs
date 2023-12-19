using System.Net;
using Microsoft.AspNetCore.Mvc;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories;
using Catalog.Host.Services.Interfaces;
using Infrastructure;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentsDefaults.DefaultRoute)]
 public class CatalogBffController : ControllerBase
 {
     private readonly ILogger<CatalogBffController> _logger;
     private readonly IBffService _service;
     
     public CatalogBffController(
         ILogger<CatalogBffController> logger,
         IBffService service)
     {
         _logger = logger;
         _service = service;
     }

	[HttpGet]
    [ProducesResponseType(typeof(PaginatedItems<CatalogItem>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> Items(int pageSize, int pageIndex)
    {
        _logger.LogInformation($"*bff-controller* request to get items by page size: {pageSize}, page index: {pageIndex}");
        var catalogItems = await _service.GetItems(pageSize, pageIndex);
        return Ok(catalogItems);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedItems<CatalogBrand>), (int) HttpStatusCode.OK)]
    public async Task<ActionResult> Brands(int pageSize, int pageIndex)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get brands by page size: {pageSize}, page index: {pageIndex}");
        var catalogBrands = await _service.GetBrands(pageSize, pageIndex);
        return Ok(catalogBrands);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedItems<CatalogType>), (int) HttpStatusCode.OK)]
    public async Task<ActionResult> Types(int pageSize, int pageIndex)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get types by page size: {pageSize}, page index: {pageIndex}");
        var catalogTypes = await _service.GetTypes(pageSize, pageIndex);
        return Ok(catalogTypes);
    }

    [HttpGet]
    public async Task<ActionResult> Item(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get item by id: {id}");
        var item = await _service.GetItem(id);
        return Ok(item);
    }
    
    [HttpGet]
    public async Task<ActionResult> Brand(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get item by id: {id}");
        var item = await _service.GetBrand(id);
        return Ok(item);
    }
    
    [HttpGet]
    public async Task<ActionResult> Type(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get item by id: {id}");
        var item = await _service.GetType(id);
        return Ok(item);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetType([FromQuery] string type)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get items by id: {type}");
        var items = await _service.GetItemByType(type);
        return Ok(items);
    }

    [HttpGet]
    public async Task<ActionResult> GetBrand([FromQuery] string brand)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get items by id: {brand}");
        var items = await _service.GetItemByBrand(brand);
        return Ok(items);
    }
    
    
}