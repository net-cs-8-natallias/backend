using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class BffService: IBffService
{
        private  readonly ILogger<BffService> _logger;
    private readonly ICatalogRepository<CatalogBrand> _brandRepository;
    private readonly ICatalogRepository<CatalogType> _typeRepository;
    private readonly IItemsCatalogRepository _itemRepository;

    public BffService(ILogger<BffService> logger,
        ICatalogRepository<CatalogBrand> brandRepository,
        ICatalogRepository<CatalogType> typeRepository,
        IItemsCatalogRepository itemRepository)
    {
        _logger = logger;
        _brandRepository = brandRepository;
        _typeRepository = typeRepository;
        _itemRepository = itemRepository;
    }
    
    public async Task<PaginatedItems<CatalogItem>> GetItems(int pageSize, int pageIndex)
    {
        var items = await _itemRepository.GetCatalog(pageSize, pageIndex);
        _logger.LogDebug($"*{GetType().Name}* found {items.TotalCount} items");
        return items;
    }

    public async Task<PaginatedItems<CatalogBrand>> GetBrands(int pageSize, int pageIndex)
    {
        var brands = await _brandRepository.GetCatalog(pageSize, pageIndex);
        _logger.LogDebug($"*{GetType().Name}* found {brands.TotalCount} brands");
        return brands;
    }

    public async Task<PaginatedItems<CatalogType>> GetTypes(int pageSize, int pageIndex)
    {
        var types = await _typeRepository.GetCatalog(pageSize, pageIndex);
        _logger.LogDebug($"*{GetType().Name}* found {types.TotalCount} types");
        return types;
    }

    public async Task<CatalogItem> GetItem(int id)
    {
        var item = await _itemRepository.FindById(id);
        _logger.LogDebug($"*{GetType().Name}* found item: {item.ToString()}");
        return item;
    }

    public async Task<CatalogBrand> GetBrand(int id)
    {
        var brand = await _brandRepository.FindById(id);
        _logger.LogDebug($"*{GetType().Name}* found brand: {brand.ToString()}");
        return brand;
    }

    public async Task<CatalogType> GetType(int id)
    {
        var type = await _typeRepository.FindById(id);
        _logger.LogDebug($"*{GetType().Name}* found type: {type.ToString()}");
        return type;
    }

    public async Task<List<CatalogItem>> GetItemByType(string type)
    {
        var items = await _itemRepository.GetItemsByType(type);
        _logger.LogDebug($"*{GetType().Name}* found item: {items.Count} by type name: {type}");
        return items;
    }

    public async Task<List<CatalogItem>> GetItemByBrand(string brand)
    {
        var items = await _itemRepository.GetItemsByBrand(brand);
        _logger.LogDebug($"*{GetType().Name}* found item: {items.Count} by brand name: {brand}");
        return items;
    }
}