using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class BrandsCatalogRepository : ICatalogRepository<CatalogBrand>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<BrandsCatalogRepository> _logger;

    public BrandsCatalogRepository(ApplicationDbContext dbContext,
        ILogger<BrandsCatalogRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<List<CatalogBrand>> GetCatalog()
    {
        return await _dbContext.CatalogBrands.ToListAsync();
    }

    public async Task<CatalogBrand> FindById(int id)
    {
        var brand = await _dbContext.CatalogBrands.FindAsync(id);
        if (brand == null)
        {
            _logger.LogError($"*{GetType().Name}* brand with id: {id} does not exist");
            throw new Exception($"Brand with ID: {id} does not exist");
        }

        return brand;
    }

    public async Task<int?> AddToCatalog(CatalogBrand catalogBrand)
    {
        var brand = await _dbContext.CatalogBrands.AddAsync(catalogBrand);
        await _dbContext.SaveChangesAsync();
        return brand.Entity.Id;
    }

    public async Task<CatalogBrand> UpdateInCatalog(CatalogBrand catalogBrand)
    {
        var brand = await FindById(catalogBrand.Id);
        brand.Brand = catalogBrand.Brand;
        brand = _dbContext.CatalogBrands.Update(brand).Entity;
        await _dbContext.SaveChangesAsync();
        return brand;
    }

    public async Task<CatalogBrand> RemoveFromCatalog(int id)
    {
        var brand = await FindById(id);
        _dbContext.CatalogBrands.Remove(brand);
        await _dbContext.SaveChangesAsync();
        return brand;
    }
}