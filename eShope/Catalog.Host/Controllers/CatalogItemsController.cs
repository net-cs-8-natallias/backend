using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Requests;
using Catalog.Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route("items")]
public class CatalogItemsController : ControllerBase
{
    private readonly ILogger<CatalogItemsController> _logger;
    private readonly ICatalogService<CatalogItem> _service;

    public CatalogItemsController(ICatalogService<CatalogItem> service,
        ILogger<CatalogItemsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> Items(int pageSize, int pageIndex)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get all items");
        var catalogItems = await _service.GetCatalog();
        return Ok(catalogItems);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Item(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get item by id: {id}");
        var item = await _service.FindById(id);
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult> Add(AddCatalogItemRequest request)
    {
        _logger.LogInformation($"*{GetType().Name}* request to add new item");
        var catalogItem = new CatalogItem
        {
            CatalogBrandId = request.CatalogBrandId,
            CatalogTypeId = request.CatalogTypeId,
            Description = request.Description,
            Name = request.Name,
            PictureFileName = request.PictureFileName,
            Price = request.Price
        };
        var id = await _service.AddToCatalog(catalogItem);
        return Ok(id);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] AddCatalogItemRequest request)
    {
        _logger.LogInformation($"*{GetType().Name}* request to update item with id: {request.Id}");
        var catalogItem = new CatalogItem
        {
            Id = request.Id,
            CatalogBrandId = request.CatalogBrandId,
            CatalogTypeId = request.CatalogTypeId,
            Description = request.Description,
            Name = request.Name,
            PictureFileName = request.PictureFileName,
            Price = request.Price
        };
        var item = await _service.UpdateInCatalog(catalogItem);
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogDebug($"*{GetType().Name}* request to delete item with id: {id}");
        var item = await _service.RemoveFromCatalog(id);
        return Ok(item);
    }
}