using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories;

public interface IItemsCatalogRepository: ICatalogRepository<CatalogItem>
{
    Task<CatalogItem[]> GetItemsByBrand(string brand);
    Task<CatalogItem[]> GetItemsByType(string type);
}