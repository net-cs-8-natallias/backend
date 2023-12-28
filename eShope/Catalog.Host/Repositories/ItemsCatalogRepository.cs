using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class ItemsCatalogRepository : IItemsCatalogRepository 
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ItemsCatalogRepository> _logger;

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

    public async Task<PaginatedItems<CatalogItem>> GetCatalog(int pageSize, int pageIndex, int brand, int type)
    {
        IQueryable<CatalogItem> query = _dbContext.CatalogItems;
        if (brand > 0)
        {
            query = query.Where(w => w.CatalogBrandId == brand);
        }
        
        if (type > 0)
        {
            query = query.Where(w => w.CatalogTypeId == type);
        }

        var catalogItems = await query
            .OrderBy(c => c.Name)
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();
        
        var totalItems = await query.LongCountAsync();
        
        return new PaginatedItems<CatalogItem>
        {
            Count = totalItems,
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
        var brand = await FindBrand(catalogItem.CatalogBrandId);
        var type =  await FindType(catalogItem.CatalogTypeId);
        var item = await _dbContext.CatalogItems.AddAsync(catalogItem);
        await _dbContext.SaveChangesAsync();
        return item.Entity.Id;
    }

    public async Task<CatalogItem> UpdateInCatalog(CatalogItem catalogItem)
    {
        var brand = await FindBrand(catalogItem.CatalogBrandId);
        var type =  await FindType(catalogItem.CatalogTypeId);
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

    private async Task<CatalogBrand> FindBrand(int id)
    {
        var brand =  await _dbContext.CatalogBrands.FindAsync(id);
        if (brand == null)
        {
            _logger.LogError($"*{GetType().Name}* brand with id: {id} does not exist");
            throw new Exception($"Brand with ID: {id} does not exist");
        }

        return brand;
    }
    
    private async Task<CatalogType> FindType(int id)
    {
        var type =  await _dbContext.CatalogTypes.FindAsync(id);
        if (type == null)
        {
            _logger.LogError($"*{GetType().Name}* type with id: {id} does not exist");
            throw new Exception($"Type with ID: {id} does not exist");
        }

        return type;
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