using Catalog.Host.Data.Entities;
using Order.Host.Data.Entities;
using Order.Host.Models;
using Order.Host.Repositories.Interfaces;
using Order.Host.Services.interfaces;

namespace Order.Host.Services;

public class OrderBffService: IOrderBffService
{
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<OrderBffService> _logger;

    private readonly ICustomerRepository<Customer, CustomerModel> _customerRepository;
    private readonly IRepository<CustomerOrder, CustomerOrderModel> _customerOrderRepository;
    private readonly IRepository<OrderItem, OrderItemModel> _orderItemRepository;

    public OrderBffService(IHttpClientService httpClient, 
        ILogger<OrderBffService> logger,
        ICustomerRepository<Customer, CustomerModel> customerRepository,
        IRepository<CustomerOrder, CustomerOrderModel> customerOrderRepository,
        IRepository<OrderItem, OrderItemModel> orderItemRepository
        )
    {
        _httpClient = httpClient;
        _logger = logger;
        _customerRepository = customerRepository;
        _customerOrderRepository = customerOrderRepository;
        _orderItemRepository = orderItemRepository;
    }
    public async Task<int> makeOrder(List<ItemModel> orderItemsModel, UserModel user)
    {
        string customerId = await ValidateCustomer(user);
        CustomerOrderModel order = new CustomerOrderModel()
            { CustomerId = customerId, Date =  DateTime.Now, TotalPrice = 0 };
        int? orderId = await _customerOrderRepository.AddItem(order);
        if (!orderId.HasValue)
        {
            throw new Exception($"order was not created");
        }
        decimal totalPrice = await AddOrderItems(orderItemsModel, orderId.Value);
        order.Id = orderId.Value;
        order.TotalPrice = totalPrice;
        await _customerOrderRepository.UpdateItem(order);
        return orderId.Value;
    }

    private async Task<decimal> AddOrderItems(List<ItemModel> orderItemsModel, int orderId)
    {
        decimal totalPrice = 0;
        foreach (var item in orderItemsModel)
        {
            var catalogItem = await _httpClient.SendAsync<CatalogItem, object>(
                $"http://localhost:5054/api/v1/CatalogBff/items/{item.Id}", 
                HttpMethod.Get, null);
            if (catalogItem == null)
            {
                throw new Exception($"item with id: {item.Id} does not exist");
            }
            Console.WriteLine($"*** catalog: {catalogItem.ToString()}");
            decimal subtotal = item.Amount * catalogItem.Price;
            totalPrice += subtotal;
            await _orderItemRepository.AddItem(new()
            {
                Amount = item.Amount,
                CatalogItemId = catalogItem.Id,
                Subtotal = subtotal,
                CustomerOrderId = orderId
            });
        }

        return totalPrice;
    }

    private async Task<string?> ValidateCustomer(UserModel user)
    {
        string id = "";
        try
        {
            Customer customer = await _customerRepository.GetItemById(user.Id);
            id = customer.Id;
        }
        catch (Exception e)
        {
            _logger.LogWarning($"Customer with id: {user.Id} does not exist");
            CustomerModel customer = new CustomerModel() { Id = user.Id, Email = user.Email, Name = user.Name };
            id = await _customerRepository.AddItem(customer);
        }

        return id;

    }
}