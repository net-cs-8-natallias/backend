using Order.Host.Models;

namespace Order.Host.Services.interfaces;

public interface IOrderBffService
{
    Task<int> makeOrder(List<ItemModel> orderItemsModel, UserModel user);
}