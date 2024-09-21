using Microsoft.EntityFrameworkCore;
using SodaBox.DataAccess.Entities;
using SodaBox.DataAccess.IRepositories;

namespace SodaBox.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly VendingMachineContext _context;

        public OrderRepository(VendingMachineContext context)
        {
            _context = context;
        }
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.orders.FindAsync(id);
        }

        // Получение всех заказов
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.orders.ToListAsync();
        }

        // Добавление нового заказа
        public async Task AddOrderAsync(DateTime orderDate, int totalAmount, List<(string brandName, string drinkName, int quantity, int price)> items)
        {
            // Создаем новый объект Order
            var order = new Order
            {
                orderDate = orderDate,
                totalAmount = totalAmount,
                orderItems = items.Select(item => new OrderItem
                {
                    brandName = item.brandName,
                    drinkName = item.drinkName,
                    quantity = item.quantity,
                    price = item.price
                }).ToList()
            };

            _context.orders.Add(order);
            await _context.SaveChangesAsync();
        }

        // Удаление заказа
        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.orders.FindAsync(id);
            if (order != null)
            {
                _context.orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        // Обновление существующего заказа
        public async Task<bool> UpdateOrderAsync(int id, DateTime orderDate, List<(string brandName, string drinkName, int quantity, double price)> items, int totalAmount)
        {
            var order = await _context.orders.Include(o => o.orderItems).FirstOrDefaultAsync(o => o.id == id);
            if (order != null)
            {
                // Обновляем информацию о заказе
                order.orderDate = orderDate;
                order.totalAmount = totalAmount;

                // Очищаем старые элементы заказа
                order.orderItems.Clear();

                // Добавляем новые элементы заказа
                order.orderItems = items.Select(item => new OrderItem
                {
                    brandName = item.brandName,
                    drinkName = item.drinkName,
                    quantity = item.quantity,
                    price = item.price
                }).ToList();

                await _context.SaveChangesAsync();
                return true;
            }

            // Если заказ не найден, возвращаем false
            return false;
        }
    }
}
