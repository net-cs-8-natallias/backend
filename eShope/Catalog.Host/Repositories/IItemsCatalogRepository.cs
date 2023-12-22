using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories;

public interface IItemsCatalogRepository : ICatalogRepository<CatalogItem>
{
    Task<List<CatalogItem>> GetItemsByBrand(string brand);
    Task<List<CatalogItem>> GetItemsByType(string type);
}