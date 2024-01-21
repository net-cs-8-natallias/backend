namespace Order.Host.Repositories.Interfaces;

public interface ICustomerRepository<T, M>
{
    Task<List<T>> GetAllItems();
    Task<T> GetItemById(string id);
    Task<string?> AddItem(M item);
    Task<T> UpdateItem(M item);
    Task<T> RemoveItem(string id);
}