using System.Net;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Requests;
using Catalog.Host.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogBrandController: ControllerBase
{
    private readonly ICatalogRepository<CatalogBrand> _catalogRepository;
    private  readonly ILogger<CatalogBrandController> _logger;
    
    public CatalogBrandController(ICatalogRepository<CatalogBrand> catalogRepository, 
        ILogger<CatalogBrandController> logger)
    {
        _catalogRepository = catalogRepository;
        _logger = logger;
    }
    
    [HttpGet("brands")]
    [ProducesResponseType(typeof(PaginatedItems<CatalogBrand>), (int) HttpStatusCode.OK)]
    public async Task<ActionResult> GetBrands(int pageSize, int pageIndex)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get brands by page size: {pageSize}, page index: {pageIndex}");
        var catalogBrands = await _catalogRepository.GetCatalog(pageSize, pageIndex);
        return Ok(catalogBrands);
    }

    [HttpGet("brands/{id}")]
    public async Task<ActionResult> GetBrandById(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get brand by id: {id}");
        var type = await _catalogRepository.FindById(id);
        return Ok(type);
    }

    [HttpPost]
    public async Task<ActionResult> AddBrand(AddCatalogBrandRequest request)
    {
        _logger.LogInformation($"*{GetType().Name}* request to add new brand");
        var catalogBrand = new CatalogBrand
        {
            Brand = request.Brand
        };
        var id = await _catalogRepository.AddToCatalog(catalogBrand);
        return Ok(id);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateBrand([FromBody]AddCatalogBrandRequest request)
    {
        _logger.LogInformation($"*{GetType().Name}* request to update brand with id: {request.Id}");
        var catalogBrand = new CatalogBrand
        {
            Id = request.Id,
            Brand = request.Brand
        };
        var brand = await _catalogRepository.UpdateInCatalog(catalogBrand);
        return Ok(brand);
    }

    [HttpDelete("brand/{id}")]
    public async Task<ActionResult> DeleteBrande(int id)
    {
        _logger.LogDebug($"*{GetType().Name}* request to delete brand with id: {id}");
        var brand = await _catalogRepository.RemoveFromCatalog(id);
        return Ok(brand);
    }
}