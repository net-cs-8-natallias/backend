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

    public async Task<List<CatalogItem>> GetCatalog()
    {
        return await _dbContext.CatalogItems.ToListAsync();
    }
    
    public async Task<PaginatedItems<CatalogItem>> GetCatalog(int pageSize, int pageIndex)
    {
        var totalItems = await _dbContext.CatalogItems.LongCountAsync();
        var catalogItems = await _dbContext.CatalogItems
        .OrderBy(c => c.Name)
        .Skip(pageSize * pageIndex)
        .Take(pageSize)
        .ToListAsync();
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
            _logger.LogError($"*{GetType().Name}* item with id: {id} does not exist");
            throw new Exception($"Item with ID: {id} does not exist");
        }
        return item;
    }
  
    public async Task<int?> AddToCatalog(CatalogItem catalogItem)
    {
        var item = await _dbContext.CatalogItems.AddAsync(catalogItem);
        await _dbContext.SaveChangesAsync();
        return item.Entity.Id;
    }

    public async Task<CatalogItem> UpdateInCatalog(CatalogItem catalogItem)
    {
        var brand = await FindById(catalogItem.CatalogBrandId);
        var type = await FindById(catalogItem.CatalogTypeId);
        var item = await FindById(catalogItem.Id);

        item.CatalogBrandId = brand.Id;
        item.CatalogTypeId = type.Id;
        item.Name = catalogItem.Name;
        item.Price = catalogItem.Price;
        item.Description = catalogItem.Description;
        item.PictureFileName = catalogItem.PictureFileName;
        
        item = _dbContext.CatalogItems.Update(item).Entity;
        await _dbContext.SaveChangesAsync();
        return item;
    }
    
    public async Task<CatalogItem> RemoveFromCatalog(int id)
    {
        var item = await FindById(id);
        _dbContext.CatalogItems.Remove(item);
        await _dbContext.SaveChangesAsync();
        return item;
    }

    public async Task<List<CatalogItem>> GetItemsByBrand(string brand)
    {
        var items = await _dbContext.CatalogItems
            .Where(item => item.CatalogBrand!.Brand == brand)
            .ToListAsync();
        return items;
    }
    
    public async Task<List<CatalogItem>> GetItemsByType(string type)
    {
        var items = await _dbContext.CatalogItems
            .Where(item => item.CatalogType!.Type == type)
            .ToListAsync();
        return items;
    }
}