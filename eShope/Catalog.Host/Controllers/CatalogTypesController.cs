using System.Net;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Requests;
using Catalog.Host.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogTypesController: ControllerBase
{
    private readonly ICatalogRepository<CatalogType> _catalogRepository;
    private  readonly ILogger<CatalogTypesController> _logger;
    
    public CatalogTypesController(ICatalogRepository<CatalogType> catalogRepository, 
        ILogger<CatalogTypesController> logger)
    {
        _catalogRepository = catalogRepository;
        _logger = logger;
    }
    
    [HttpGet("GetTypes")]
    [ProducesResponseType(typeof(PaginatedItems<CatalogType>), (int) HttpStatusCode.OK)]
    public async Task<ActionResult> GetTypes(int pageSize, int pageIndex)
    {
        _logger.LogInformation($"*types-controller* request to get types by page size: {pageSize}, page index: {pageIndex}");
        var catalogTypes = await _catalogRepository.GetCatalog(pageSize, pageIndex);
        return Ok(catalogTypes);
    }

    [HttpGet("GetItemsById/{id}")]
    public async Task<ActionResult> GetItemById(int id)
    {
        _logger.LogInformation($"*types-controller* request to get type by id: {id}");
        var type = await _catalogRepository.FindById(id);
        return Ok(type);
    }

    [HttpPost]
    public async Task<ActionResult> AddType(AddCatalogTypeRequest request)
    {
        _logger.LogInformation("*types-controller* request to add new type");
        var catalogType = new CatalogType
        {
            Type = request.Type
        };
        var id = await _catalogRepository.AddToCatalog(catalogType);
        return Ok(id);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateType([FromBody]AddCatalogTypeRequest request)
    {
        _logger.LogInformation($"*types-controller* request to update type with id: {request.Id}");
        var catalogType = new CatalogType
        {
            Id = request.Id,
            Type = request.Type
        };
        var type = await _catalogRepository.UpdateInCatalog(catalogType);
        return Ok(type);
    }

    [HttpDelete("type/{id}")]
    public async Task<ActionResult> DeleteType(int id)
    {
        _logger.LogDebug($"*types-controller* request to delete item with id: {id}");
        var type = await _catalogRepository.RemoveFromCatalog(id);
        return Ok(type);
    }
}