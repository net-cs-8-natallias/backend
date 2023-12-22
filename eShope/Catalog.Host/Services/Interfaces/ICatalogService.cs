namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService<T>
{
    Task<List<T>> GetCatalog();
    Task<T> FindById(int id);
    Task<int?> AddToCatalog(T catalog);
    Task<T> UpdateInCatalog(T catalog);
    Task<T> RemoveFromCatalog(int id);
}