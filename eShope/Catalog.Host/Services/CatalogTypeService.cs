using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogTypeService : ICatalogService<CatalogType>
{
    private readonly ILogger<CatalogTypeService> _logger;
    private readonly ICatalogRepository<CatalogType> _repository;

    public CatalogTypeService(ILogger<CatalogTypeService> logger,
        ICatalogRepository<CatalogType> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<List<CatalogType>> GetCatalog()
    {
        var types = await _repository.GetCatalog();
        _logger.LogDebug($"*{GetType().Name}* found {types.Count} types");
        return types;
    }

    public async Task<CatalogType> FindById(int id)
    {
        var type = await _repository.FindById(id);
        _logger.LogDebug($"*{GetType().Name}* found type {type}");
        return type;
    }

    public async Task<int?> AddToCatalog(CatalogType catalog)
    {
        var typeId = await _repository.AddToCatalog(catalog);
        _logger.LogDebug($"*{GetType().Name}* new catalog was added with id: {typeId}");
        return typeId;
    }

    public async Task<CatalogType> UpdateInCatalog(CatalogType catalog)
    {
        var type = await _repository.UpdateInCatalog(catalog);
        _logger.LogDebug($"*{GetType().Name}* type with id: {type.Id} was updated");
        return type;
    }

    public async Task<CatalogType> RemoveFromCatalog(int id)
    {
        var type = await _repository.RemoveFromCatalog(id);
        _logger.LogDebug($"*{GetType().Name}* type with id: {type.Id} was removed");
        return type;
    }
}