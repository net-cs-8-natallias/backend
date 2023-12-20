using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Requests;
using Catalog.Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route("types")]
public class CatalogTypesController : ControllerBase
{
    private readonly ILogger<CatalogTypesController> _logger;
    private readonly ICatalogService<CatalogType> _service;

    public CatalogTypesController(ICatalogService<CatalogType> service,
        ILogger<CatalogTypesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> Types()
    {
        _logger.LogInformation($"*{GetType().Name}* request to get all types");
        var catalogTypes = await _service.GetCatalog();
        return Ok(catalogTypes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Type(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get type by id: {id}");
        var type = await _service.FindById(id);
        return Ok(type);
    }

    [HttpPost]
    public async Task<ActionResult> Add(AddCatalogTypeRequest request)
    {
        _logger.LogInformation($"*{GetType().Name}* request to add new type");
        var catalogType = new CatalogType
        {
            Type = request.Type
        };
        var id = await _service.AddToCatalog(catalogType);
        return Ok(id);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] AddCatalogTypeRequest request)
    {
        _logger.LogInformation($"*{GetType().Name}* request to update type with id: {request.Id}");
        var catalogType = new CatalogType
        {
            Id = request.Id,
            Type = request.Type
        };
        var type = await _service.UpdateInCatalog(catalogType);
        return Ok(type);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogDebug($"*{GetType().Name}* request to delete item with id: {id}");
        var type = await _service.RemoveFromCatalog(id);
        return Ok(type);
    }
}