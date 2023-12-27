using Catalog.Host.Data;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories;

public interface IItemsCatalogRepository : ICatalogRepository<CatalogItem>
{
    Task<PaginatedItems<CatalogItem>> GetCatalog(int pageSize, int pageIndex, int brand, int type);
    Task<List<CatalogItem>> GetItemsByBrand(string brand);
    Task<List<CatalogItem>> GetItemsByType(string type);
}