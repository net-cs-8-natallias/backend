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

    public async Task<PaginatedItems<CatalogItem>> GetItems(int pageSize, int pageIndex)
    {
        var items = await _itemRepository.GetCatalog(pageSize, pageIndex);
        _logger.LogDebug($"*{GetType().Name}* found {items.TotalCount} items");
        return new PaginatedItems<CatalogItem>()
        {
            TotalCount = items.TotalCount,
            Data = items.Data.Select(s => _mapper
                .Map<CatalogItem>(s)).ToList()
        };
    }

    public async Task<PaginatedItems<CatalogBrand>> GetBrands(int pageSize, int pageIndex)
    {
        var brands = await _brandRepository.GetCatalog(pageSize, pageIndex);
        _logger.LogDebug($"*{GetType().Name}* found {brands.TotalCount} brands");
        return new PaginatedItems<CatalogBrand>()
        {
            TotalCount = brands.TotalCount,
            Data = brands.Data.Select(s => _mapper
                .Map<CatalogBrand>(s)).ToList()
        };
    }

    public async Task<PaginatedItems<CatalogType>> GetTypes(int pageSize, int pageIndex)
    {
        var types = await _typeRepository.GetCatalog(pageSize, pageIndex);
        _logger.LogDebug($"*{GetType().Name}* found {types.TotalCount} types");
        return new PaginatedItems<CatalogType>()
        {
            TotalCount = types.TotalCount,
            Data = types.Data.Select(s => _mapper
                .Map<CatalogType>(s)).ToList()
        };
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