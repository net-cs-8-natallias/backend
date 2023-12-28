using AutoMapper;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class BffService : IBffService
{
    private readonly ICatalogRepository<CatalogBrand> _brandRepository;
    private readonly IItemsCatalogRepository _itemRepository;
    private readonly ILogger<BffService> _logger;
    private readonly ICatalogRepository<CatalogType> _typeRepository;
    private readonly IMapper _mapper;

    public BffService(ILogger<BffService> logger,
        ICatalogRepository<CatalogBrand> brandRepository,
        ICatalogRepository<CatalogType> typeRepository,
        IItemsCatalogRepository itemRepository,
        IMapper mapper)
    {
        _logger = logger;
        _brandRepository = brandRepository;
        _typeRepository = typeRepository;
        _itemRepository = itemRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedItems<CatalogItem>> GetItems(int pageSize, int pageIndex, int brand, int type)
    {
        var items = await _itemRepository.GetCatalog(pageSize, pageIndex, brand, type);
        _logger.LogDebug($"*{GetType().Name}* found {items.Count} items");
        return new PaginatedItems<CatalogItem>()
        {
            Count = items.Count,
            Data = items.Data.Select(s => _mapper
                .Map<CatalogItem>(s)).ToList(),
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }
    
    public async Task<List<CatalogItem>> GetItems()
    {
        var items = await _itemRepository.GetCatalog();
        _logger.LogDebug($"*{GetType().Name}* found {items.Count} items");
        return items;
    }
    
    public async Task<List<CatalogBrand>> GetBrands()
    {
        var brands = await _brandRepository.GetCatalog();
        _logger.LogDebug($"*{GetType().Name}* found {brands.Count} brands");
        return brands;
    }
    
    public async Task<List<CatalogType>> GetTypes()
    {
        var types = await _typeRepository.GetCatalog();
        _logger.LogDebug($"*{GetType().Name}* found {types.Count} types");
        return types;
    }
    
    public async Task<CatalogItem> GetItem(int id)
    {
        var item = await _itemRepository.FindById(id);
        _logger.LogDebug($"*{GetType().Name}* found item: {item}");
        return item;
    }

    public async Task<CatalogBrand> GetBrand(int id)
    {
        var brand = await _brandRepository.FindById(id);
        _logger.LogDebug($"*{GetType().Name}* found brand: {brand}");
        return brand;
    }

    public async Task<CatalogType> GetType(int id)
    {
        var type = await _typeRepository.FindById(id);
        _logger.LogDebug($"*{GetType().Name}* found type: {type}");
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