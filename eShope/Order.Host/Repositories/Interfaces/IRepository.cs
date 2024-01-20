namespace Order.Host.Repositories.Interfaces;

public interface IRepository<T, M>
{
    Task<List<T>> GetAllItems();
    Task<T> GetItemById(int id);
    Task<int?> AddItem(M item);
    Task<T> UpdateItem(M item);
    Task<T> RemoveItem(int id);
}