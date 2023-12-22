using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogBrandService : ICatalogService<CatalogBrand>
{
    private readonly ILogger<CatalogBrandService> _logger;
    private readonly ICatalogRepository<CatalogBrand> _repository;

    public CatalogBrandService(ILogger<CatalogBrandService> logger,
        ICatalogRepository<CatalogBrand> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<List<CatalogBrand>> GetCatalog()
    {
        var brands = await _repository.GetCatalog();
        _logger.LogDebug($"*{GetType().Name}* found {brands.Count} brands");
        return brands;
    }

    public async Task<CatalogBrand> FindById(int id)
    {
        var brand = await _repository.FindById(id);
        _logger.LogDebug($"*{GetType().Name}* found brand {brand}");
        return brand;
    }

    public async Task<int?> AddToCatalog(CatalogBrand catalog)
    {
        var brandId = await _repository.AddToCatalog(catalog);
        _logger.LogDebug($"*{GetType().Name}* new catalog was added with id: {brandId}");
        return brandId;
    }

    public async Task<CatalogBrand> UpdateInCatalog(CatalogBrand catalog)
    {
        var brand = await _repository.UpdateInCatalog(catalog);
        _logger.LogDebug($"*{GetType().Name}* brand with id: {brand.Id} was updated");
        return brand;
    }

    public async Task<CatalogBrand> RemoveFromCatalog(int id)
    {
        var brand = await _repository.RemoveFromCatalog(id);
        _logger.LogDebug($"*{GetType().Name}* brand with id: {brand.Id} was removed");
        return brand;
    }
}