namespace Order.Host.Services.interfaces;

public interface IOrderService<T, M>
{
    Task<List<T>> GetAllItems();
    Task<T> GetItemById(int id);
    Task<int?> AddItem(M item);
    Task<T> UpdateItem(M item);
    Task<T> RemoveItem(int id);
}