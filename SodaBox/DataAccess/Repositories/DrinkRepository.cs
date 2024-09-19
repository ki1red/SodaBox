using Microsoft.EntityFrameworkCore;
using SodaBox.DataAccess.Entities;
using SodaBox.DataAccess.IRepositories;
using System.Threading.Tasks;

namespace SodaBox.DataAccess.Repositories
{
    public class DrinkRepository : IDrinkRepository
    {
        private readonly VendingMachineContext _context;

        public DrinkRepository(VendingMachineContext context)
        {
            _context = context;
        }

        // Получить напиток по id
        public async Task<Drink> GetDrinkByIdAsync(int id)
        {
            return await _context.drinks.FindAsync(id);
        }

        // Получить все напитки
        public async Task<IEnumerable<Drink>> GetAllDrinksAsync()
        {
            return await _context.drinks.ToListAsync();
        }

        public async Task<IEnumerable<Drink>> SortByDrinksAsync(int brandId, int minPrice, int maxPrice)
        {
            Console.WriteLine($"{brandId} {minPrice} {maxPrice}");
            var drinks = await GetAllDrinksAsync();
            Console.WriteLine(drinks.ToList().Count);
            if (brandId > 0)
            {
                drinks = drinks.Where(item => item.brandId == brandId);
            }
            Console.WriteLine(drinks.ToList().Count);
            if (minPrice >= 0 && minPrice <= maxPrice)
            {
                drinks = drinks.Where(item => item.price >= minPrice);
            }
            Console.WriteLine(drinks.ToList().Count);
            if (maxPrice >= 0 && maxPrice >= minPrice)
            {
                drinks = drinks.Where(item => item.price <= maxPrice);
            }
            Console.WriteLine(drinks.ToList().Count);
            return drinks;
        }

        // Обновить цену напитка
        public async Task UpdatePriceAsync(int drinkId, int newPrice)
        {
            var drink = await GetDrinkByIdAsync(drinkId);
            if (drink != null)
            {
                drink.price = newPrice;
                await SaveChangesAsync();
            }
        }

        // Обновить количество напитка
        public async Task UpdateQuantityAsync(int drinkId, int newQuantity)
        {
            var drink = await GetDrinkByIdAsync(drinkId);

            if (newQuantity < 0)
                newQuantity = 0;

            if (drink != null)
            {
                drink.quantity = newQuantity;
                await SaveChangesAsync();
            }
        }

        // Обновить позицию (id) напитка
        public async Task SwapPositionsAsync(int firstId, int secondId)
        {
            // Получаем напитки по их идентификаторам
            var firstDrink = await GetDrinkByIdAsync(firstId);
            var secondDrink = await GetDrinkByIdAsync(secondId);

            // Проверяем, что оба напитка существуют
            if (firstDrink != null && secondDrink != null)
            {
                // Временная переменная для хранения ID
                int tempId = firstDrink.id;

                // Меняем идентификаторы местами
                firstDrink.id = secondDrink.id;
                secondDrink.id = tempId;

                // Сохраняем изменения в базе данных
                await SaveChangesAsync();
            }
        }


        // Сохранить изменения в базе данных
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
