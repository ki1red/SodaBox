using SodaBox.DataAccess.Entities;

namespace SodaBox.DataAccess.IRepositories
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task AddOrderAsync(DateTime orderDate, int totalAmount, List<(string brandName, string drinkName, int quantity, int price)> items);
        Task DeleteOrderAsync(int id);
        Task<bool> UpdateOrderAsync(int id, DateTime orderDate, List<(string brandName, string drinkName, int quantity, double price)> items, int totalAmount);
    }
}
