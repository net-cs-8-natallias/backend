using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class ItemsCatalogRepository: IItemsCatalogRepository//ICatalogRepository<CatalogItem>
{

    private readonly ApplicationDbContext _dbContext;
    private  readonly ILogger<ItemsCatalogRepository> _logger;
    public ItemsCatalogRepository(ApplicationDbContext dbContext,
        ILogger<ItemsCatalogRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<PaginatedItems<CatalogItem>> GetCatalog(int pageSize, int pageIndex)
    {
        var totalItems = await _dbContext.CatalogItems.LongCountAsync();
        _logger.LogDebug($"*items-repo* found total items: {totalItems}");
        var catalogItems = await _dbContext.CatalogItems
        .OrderBy(c => c.Name)
        .Skip(pageSize * pageIndex)
        .Take(pageSize)
        .ToListAsync();
        _logger.LogDebug($"*items-repo* return {catalogItems.Count} items");
        return new PaginatedItems<CatalogItem>
        {
            TotalCount = totalItems,
            Data = catalogItems
        };
    }

    public async Task<CatalogItem> FindById(int id)
    {
        var item = await _dbContext.CatalogItems.FindAsync(id);
        if (item == null)
        {
            _logger.LogError($"*items-repo* item with id: {id} does not exist");
            throw new Exception($"Item with ID: {id} does not exist");
        }
        _logger.LogDebug($"*items-repo* found item: {item.Id}");
        return item;
    }
  
    public async Task<int?> AddToCatalog(CatalogItem catalogItem)
    {
        var item = await _dbContext.CatalogItems.AddAsync(catalogItem);
        await _dbContext.SaveChangesAsync();
        _logger.LogDebug($"*items-repo* new item was added: {item.Entity.Id}");
        return item.Entity.Id;
    }

    public async Task<CatalogItem> UpdateInCatalog(CatalogItem catalogItem)
    {
        var brand = await _dbContext.CatalogBrands.FindAsync(catalogItem.CatalogBrandId);
        if (brand == null)
        {
            _logger.LogError($"*items-repo* catalog-brand with brand-id: {catalogItem.CatalogBrandId} does not exist");
            throw new Exception($"Brand with brand-ID: {catalogItem.CatalogBrandId} does not exist");
        }

        var type = await _dbContext.CatalogTypes.FindAsync(catalogItem.CatalogTypeId);
        if (type == null)
        {
            _logger.LogError($"*items-repo* catalog-type with type-id: {catalogItem.CatalogTypeId} does not exist");
            throw new Exception($"Type with type-ID: {catalogItem.CatalogTypeId} does not exist");
        }

        var item = await _dbContext.CatalogItems.FindAsync(catalogItem.Id);
        if (item == null)
        {
            _logger.LogError($"*items-repo* catalog-item with item-id: {catalogItem.Id} does not exist");
            throw new Exception($"Item with item-ID: {catalogItem.Id} does not exist");
        }
        item.CatalogBrandId = brand.Id;
        item.CatalogTypeId = type.Id;
        item.Name = catalogItem.Name;
        item.Price = catalogItem.Price;
        item.Description = catalogItem.Description;
        item.PictureFileName = catalogItem.PictureFileName;
        
        item = _dbContext.CatalogItems.Update(item).Entity;
        await _dbContext.SaveChangesAsync();
        _logger.LogDebug($"*items-repo* item: {item.Id} was updated");
        return item;
    }
    
    public async Task<CatalogItem> RemoveFromCatalog(int id)
    {
        var item = await FindById(id);
        _dbContext.Remove(item);
        await _dbContext.SaveChangesAsync();
        _logger.LogDebug($"*items-repo* item with id: {id} was removed");
        return item;
    }

    public async Task<CatalogItem[]> GetItemsByBrand(string brand)
    {
        var items = await _dbContext.CatalogItems
            .Where(item => item.CatalogBrand.Brand == brand)
            .ToArrayAsync();
        _logger.LogDebug($"*items-repo* found {items.Length} items");
        return items;
    }
    
    public async Task<CatalogItem[]> GetItemsByType(string type)
    {
        var items = await _dbContext.CatalogItems
            .Where(item => item.CatalogType.Type == type)
            .ToArrayAsync();
        _logger.LogDebug($"*items-repo* found {items.Length} items");
        return items;
    }
}