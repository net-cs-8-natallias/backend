using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Requests;
using Catalog.Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route("brands")]
public class CatalogBrandController : ControllerBase
{
    private readonly ILogger<CatalogBrandController> _logger;
    private readonly ICatalogService<CatalogBrand> _service;

    public CatalogBrandController(ICatalogService<CatalogBrand> service,
        ILogger<CatalogBrandController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> Brands()
    {
        _logger.LogInformation($"*{GetType().Name}* request to get all brands");
        var catalogBrands = await _service.GetCatalog();
        return Ok(catalogBrands);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Brand(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get brand by id: {id}");
        var type = await _service.FindById(id);
        return Ok(type);
    }

    [HttpPost]
    public async Task<ActionResult> Add(AddCatalogBrandRequest request)
    {
        _logger.LogInformation($"*{GetType().Name}* request to add new brand");
        var catalogBrand = new CatalogBrand
        {
            Brand = request.Brand
        };
        var id = await _service.AddToCatalog(catalogBrand);
        return Ok(id);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] AddCatalogBrandRequest request)
    {
        _logger.LogInformation($"*{GetType().Name}* request to update brand with id: {request.Id}");
        var catalogBrand = new CatalogBrand
        {
            Id = request.Id,
            Brand = request.Brand
        };
        var brand = await _service.UpdateInCatalog(catalogBrand);
        return Ok(brand);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogDebug($"*{GetType().Name}* request to delete brand with id: {id}");
        var brand = await _service.RemoveFromCatalog(id);
        return Ok(brand);
    }
}