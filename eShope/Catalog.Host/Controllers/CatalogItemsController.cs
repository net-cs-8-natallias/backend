using System.Net;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Requests;
using Catalog.Host.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogItemsController: ControllerBase
{
    private readonly ICatalogRepository<CatalogItem> _catalogRepository;
    private  readonly ILogger<CatalogItemsController> _logger;
    
    public CatalogItemsController(ICatalogRepository<CatalogItem> catalogRepository, 
        ILogger<CatalogItemsController> logger)
    {
        _catalogRepository = catalogRepository;
        _logger = logger;
    }
    
    [HttpGet("GetItems")]
    [ProducesResponseType(typeof(PaginatedItems<CatalogItem>), (int) HttpStatusCode.OK)]
    public async Task<ActionResult> GetItems(int pageSize, int pageIndex)
    {
        _logger.LogInformation($"*item-controller* request to get items by page size: {pageSize}, page index: {pageIndex}");
        var catalogItems = await _catalogRepository.GetCatalog(pageSize, pageIndex);
        return Ok(catalogItems);
    }

    [HttpGet("GetItemsById/{id}")]
    public async Task<ActionResult> GetItemById(int id)
    {
        _logger.LogInformation($"*item-controller* request to get item by id: {id}");
        var item = await _catalogRepository.FindById(id);
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult> AddItem(AddCatalogItemRequest request)
    {
        _logger.LogInformation("*item-controller* request to add new item");
        var catalogItem = new CatalogItem
        {
            CatalogBrandId = request.CatalogBrandId,
            CatalogTypeId = request.CatalogTypeId,
            Description = request.Description,
            Name = request.Name,
            PictureFileName = request.PictureFileName, 
            Price = request.Price
        };
        var id = await _catalogRepository.AddToCatalog(catalogItem);
        return Ok(id);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateItem([FromBody]AddCatalogItemRequest request)
    {
        _logger.LogInformation($"*item-controller* request to update item with id: {request.Id}");
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
        var item = await _catalogRepository.UpdateInCatalog(catalogItem);
        return Ok(item);
    }

    [HttpDelete("item/{id}")]
    public async Task<ActionResult> DeleteItem(int id)
    {
        _logger.LogDebug($"*item-controller* request to delete item with id: {id}");
        var item = await _catalogRepository.RemoveFromCatalog(id);
        return Ok(item);
    }
}