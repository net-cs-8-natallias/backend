using System.Net;
using Microsoft.AspNetCore.Mvc;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories;
using Infrastructure;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentsDefaults.DefaultRoute)]
 public class CatalogBffController : ControllerBase
 {
     private readonly ILogger<CatalogBffController> _logger;
     private readonly IItemsCatalogRepository _itemRepository;
     private readonly ICatalogRepository<CatalogBrand> _brandRepository;
     private readonly ICatalogRepository<CatalogType> _typeRepository;
     
     public CatalogBffController(
         ILogger<CatalogBffController> logger,
         IItemsCatalogRepository itemRepository, 
         ICatalogRepository<CatalogBrand> brandRepository, 
         ICatalogRepository<CatalogType> typeRepository)
     {
         _logger = logger;
         _itemRepository = itemRepository;
         _brandRepository = brandRepository;
         _typeRepository = typeRepository;
     }

	[HttpGet]
    [ProducesResponseType(typeof(PaginatedItems<CatalogItem>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> Items(int pageSize, int pageIndex)
    {
        _logger.LogInformation($"*bff-controller* request to get items by page size: {pageSize}, page index: {pageIndex}");
        var catalogItems = await _itemRepository.GetCatalog(pageSize, pageIndex);
        return Ok(catalogItems);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedItems<CatalogBrand>), (int) HttpStatusCode.OK)]
    public async Task<ActionResult> Brands(int pageSize, int pageIndex)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get brands by page size: {pageSize}, page index: {pageIndex}");
        var catalogBrands = await _brandRepository.GetCatalog(pageSize, pageIndex);
        return Ok(catalogBrands);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedItems<CatalogType>), (int) HttpStatusCode.OK)]
    public async Task<ActionResult> Types(int pageSize, int pageIndex)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get types by page size: {pageSize}, page index: {pageIndex}");
        var catalogTypes = await _typeRepository.GetCatalog(pageSize, pageIndex);
        return Ok(catalogTypes);
    }

    [HttpGet]
    public async Task<ActionResult> Item(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get item by id: {id}");
        var item = await _itemRepository.FindById(id);
        return Ok(item);
    }
    
    [HttpGet]
    public async Task<ActionResult> Brand(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get item by id: {id}");
        var item = await _brandRepository.FindById(id);
        return Ok(item);
    }
    
    [HttpGet]
    public async Task<ActionResult> Type(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get item by id: {id}");
        var item = await _typeRepository.FindById(id);
        return Ok(item);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetByType(string type)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get items by id: {type}");
        var items = await _itemRepository.GetItemsByType(type);
        return Ok(items);
    }

    [HttpGet]
    public async Task<ActionResult> GetByBrand(string brand)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get items by id: {brand}");
        var items = await _itemRepository.GetItemsByBrand(brand);
        return Ok(items);
    }
    
    
}