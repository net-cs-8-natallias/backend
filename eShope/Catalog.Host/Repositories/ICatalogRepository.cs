using Catalog.Host.Data;

namespace Catalog.Host.Repositories;

public interface ICatalogRepository<T>
{
    Task<List<T>> GetCatalog();
    Task<PaginatedItems<T>> GetCatalog(int pageSize, int pageIndex);
    Task<T> FindById(int id);
    Task<int?> AddToCatalog(T catalogItem);
    Task<T> UpdateInCatalog(T catalogItem);
    Task<T> RemoveFromCatalog(int id);
}