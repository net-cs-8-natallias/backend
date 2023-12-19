using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogItemService: ICatalogService<CatalogItem>
{
    private  readonly ILogger<CatalogItemService> _logger;
    private readonly ICatalogRepository<CatalogItem> _repository;

    public CatalogItemService(ILogger<CatalogItemService> logger, 
        ICatalogRepository<CatalogItem> repository)
    {
        _logger = logger;
        _repository = repository;
    }
    
    public async Task<List<CatalogItem>> GetCatalog()
    {
        var items = await _repository.GetCatalog();
        _logger.LogDebug($"*{GetType().Name}* found {items.Count} items");
        return items;
    }

    public async Task<CatalogItem> FindById(int id)
    {
        var item = await _repository.FindById(id);
        _logger.LogDebug($"*{GetType().Name}* found item {item.ToString()}");
        return item;
    }

    public async Task<int?> AddToCatalog(CatalogItem catalog)
    {
        var itemId = await _repository.AddToCatalog(catalog);
        _logger.LogDebug($"*{GetType().Name}* new catalog was added with id: {itemId}");
        return itemId;
    }

    public async Task<CatalogItem> UpdateInCatalog(CatalogItem catalog)
    {
        var item = await _repository.UpdateInCatalog(catalog);
        _logger.LogDebug($"*{GetType().Name}* item with id: {item.Id} was updated");
        return item;
    }

    public async Task<CatalogItem> RemoveFromCatalog(int id)
    {
        var item = await _repository.RemoveFromCatalog(id);
        _logger.LogDebug($"*{GetType().Name}* item with id: {item.Id} was removed");
        return item;
    }
}