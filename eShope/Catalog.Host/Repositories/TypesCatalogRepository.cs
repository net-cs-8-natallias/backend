using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class TypesCatalogRepository: ICatalogRepository<CatalogType>
{
    private readonly ApplicationDbContext _dbContext;
    private  readonly ILogger<TypesCatalogRepository> _logger;
    public TypesCatalogRepository(ApplicationDbContext dbContext,
        ILogger<TypesCatalogRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<List<CatalogType>> GetCatalog()
    {
        return await _dbContext.CatalogTypes.ToListAsync();
    }
    public async Task<PaginatedItems<CatalogType>> GetCatalog(int pageSize, int pageIndex)
    {
        var totalTypes = await _dbContext.CatalogTypes.LongCountAsync();
        var catalogTypes = await _dbContext.CatalogTypes
            .OrderBy(c => c.Type)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();
        return new PaginatedItems<CatalogType>
        {
            TotalCount = totalTypes,
            Data = catalogTypes
        };
    }

    public async Task<CatalogType> FindById(int id)
    {
        var type = await _dbContext.CatalogTypes.FindAsync(id);
        if (type == null)
        {
            _logger.LogError($"*{GetType().Name}* type with id: {id} does not exist");
            throw new Exception($"Type with ID: {id} does not exist");
        }
        return type;
    }

    public async Task<int?> AddToCatalog(CatalogType catalogType)
    {
        var type = await _dbContext.CatalogTypes.AddAsync(catalogType);
        await _dbContext.SaveChangesAsync();
        return type.Entity.Id;
    }

    public async Task<CatalogType> UpdateInCatalog(CatalogType catalogType)
    {
        var type = await FindById(catalogType.Id);
        type.Type = catalogType.Type;
        type = _dbContext.CatalogTypes.Update(type).Entity;
        await _dbContext.SaveChangesAsync();
        return type;
    }

    public async Task<CatalogType> RemoveFromCatalog(int id)
    {
        var type = await FindById(id);
        _dbContext.CatalogTypes.Remove(type);
        await _dbContext.SaveChangesAsync();
        return type;
    }
}