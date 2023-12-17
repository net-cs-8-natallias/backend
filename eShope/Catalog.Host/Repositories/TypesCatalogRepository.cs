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
    
    public async Task<PaginatedItems<CatalogType>> GetCatalog(int pageSize, int pageIndex)
    {
        var totalTypes = await _dbContext.CatalogTypes.LongCountAsync();
        _logger.LogDebug($"*{GetType().Name}* found total types: {totalTypes}");
        var catalogTypes = await _dbContext.CatalogTypes
            .OrderBy(c => c.Type)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();
        _logger.LogDebug($"*{GetType().Name}* return {catalogTypes.Count} types");
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
        _logger.LogDebug($"*{GetType().Name}* found type: {type.Id}");
        return type;
    }

    public async Task<int?> AddToCatalog(CatalogType catalogType)
    {
        var type = await _dbContext.CatalogTypes.AddAsync(catalogType);
        await _dbContext.SaveChangesAsync();
        _logger.LogDebug($"*{GetType().Name}* new type was added: {type.Entity.Id}");
        return type.Entity.Id;
    }

    public async Task<CatalogType> UpdateInCatalog(CatalogType catalogType)
    {
        var type = await _dbContext.CatalogTypes.FindAsync(catalogType.Id);
        if (type == null)
        {
            _logger.LogError($"*{GetType().Name}* catalog-type with type-id: {catalogType.Id} does not exist");
            throw new Exception($"Type with type-ID: {catalogType.Id} does not exist");
        }

        type.Type = catalogType.Type;

        type = _dbContext.CatalogTypes.Update(type).Entity;
        await _dbContext.SaveChangesAsync();
        _logger.LogDebug($"*{GetType().Name}* type: {type.Id} was updated");
        return type;
    }

    public async Task<CatalogType> RemoveFromCatalog(int id)
    {
        var type = await FindById(id);
        _dbContext.Remove(type);
        await _dbContext.SaveChangesAsync();
        _logger.LogDebug($"*{GetType().Name}* type with id: {id} was removed");
        return type;
    }
}