using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class BrandsCatalogRepository: ICatalogRepository<CatalogBrand>
{
    private readonly ApplicationDbContext _dbContext;
    private  readonly ILogger<BrandsCatalogRepository> _logger;
    public BrandsCatalogRepository(ApplicationDbContext dbContext,
        ILogger<BrandsCatalogRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<PaginatedItems<CatalogBrand>> GetCatalog(int pageSize, int pageIndex)
    {
        var totalBrands = await _dbContext.CatalogBrands.LongCountAsync();
        _logger.LogDebug($"*{GetType().Name}* found total brands: {totalBrands}");
        var catalogBrands = await _dbContext.CatalogBrands
            .OrderBy(c => c.Brand)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();
        _logger.LogDebug($"*{GetType().Name}* return {catalogBrands.Count} brands");
        return new PaginatedItems<CatalogBrand>
        {
            TotalCount = totalBrands,
            Data = catalogBrands
        };
    }

    public async Task<CatalogBrand> FindById(int id)
    {
        var brand = await _dbContext.CatalogBrands.FindAsync(id);
        if (brand == null)
        {
            _logger.LogError($"*{GetType().Name}* brand with id: {id} does not exist");
            throw new Exception($"Brand with ID: {id} does not exist");
        }
        _logger.LogDebug($"*{GetType().Name}* found brand: {brand.Id}");
        return brand;
    }

    public async Task<int?> AddToCatalog(CatalogBrand catalogBrand)
    {
        var brand = await _dbContext.CatalogBrands.AddAsync(catalogBrand);
        await _dbContext.SaveChangesAsync();
        _logger.LogDebug($"*{GetType().Name}* new brand was added: {brand.Entity.Id}");
        return brand.Entity.Id;
    }

    public async Task<CatalogBrand> UpdateInCatalog(CatalogBrand catalogBrand)
    {
        var brand = await _dbContext.CatalogBrands.FindAsync(catalogBrand.Id);
        if (brand == null)
        {
            _logger.LogError($"*{GetType().Name}* catalog-brand with brand-id: {catalogBrand.Id} does not exist");
            throw new Exception($"Brand with brand-ID: {catalogBrand.Id} does not exist");
        }

        brand.Brand = catalogBrand.Brand;

        brand = _dbContext.CatalogBrands.Update(brand).Entity;
        await _dbContext.SaveChangesAsync();
        _logger.LogDebug($"*{GetType().Name}* brand: {brand.Id} was updated");
        return brand;
    }

    public async Task<CatalogBrand> RemoveFromCatalog(int id)
    {
        var brand = await FindById(id);
        _dbContext.Remove(brand);
        await _dbContext.SaveChangesAsync();
        _logger.LogDebug($"*{GetType().Name}* brand with id: {id} was removed");
        return brand;
    }
}