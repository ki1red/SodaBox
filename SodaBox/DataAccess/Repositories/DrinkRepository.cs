using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SodaBox.DataAccess.Entities;
using SodaBox.DataAccess.IRepositories;
using System.Collections.Generic;
using System.Diagnostics;
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
            var drinks = await GetAllDrinksAsync();
            if (brandId > 0)
            {
                drinks = drinks.Where(item => item.brandId == brandId);
            }
            if (minPrice >= 0 && minPrice <= maxPrice)
            {
                drinks = drinks.Where(item => item.price >= minPrice);
            }
            if (maxPrice >= 0 && maxPrice >= minPrice)
            {
                drinks = drinks.Where(item => item.price <= maxPrice);
            }
            return drinks;
        }

        // Обновить цену напитка
        public async Task<bool> UpdatePriceAsync(int drinkId, int newPrice)
        {
            var drink = await GetDrinkByIdAsync(drinkId);
            if (drink != null && newPrice > 0)
            {
                drink.price = newPrice;
                await SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Обновить количество напитка
        public async Task<bool> UpdateQuantityAsync(int drinkId, int newQuantity)
        {
            var drink = await GetDrinkByIdAsync(drinkId);

            if (drink != null && newQuantity >= 0)
            {
                drink.quantity = newQuantity;
                await SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAllDrinksPositions(Dictionary<int, int> newPositions)
        {
            var drinks = (await GetAllDrinksAsync()).ToList();
            if (newPositions == null || newPositions.Count != drinks.Count())
                return false;

            var oldDrinks = drinks.Select(d => new Drink
            {
                id = d.id,
                name = d.name,
                price = d.price,
                quantity = d.quantity,
                brandId = d.brandId,
                brand = null,
                imagePath = d.imagePath
            }).ToList();

            foreach (var pos in newPositions)
            {
                drinks[pos.Key-1].name = oldDrinks[pos.Value-1].name;
                drinks[pos.Key-1].price = oldDrinks[pos.Value-1].price;
                drinks[pos.Key-1].quantity = oldDrinks[pos.Value-1].quantity;
                drinks[pos.Key-1].brandId = oldDrinks[pos.Value-1].brandId;
                drinks[pos.Key-1].imagePath = oldDrinks[pos.Value-1].imagePath;
            }
            
            await SaveChangesAsync();

            return true;
        }

        // Обновить позицию (id) напитка
        public async Task<bool> SwapPositionsAsync(int firstId, int secondId)
        {
            if (firstId == secondId)
                return true;

            // Получаем напитки по их идентификаторам
            var firstDrink = await GetDrinkByIdAsync(firstId);
            var secondDrink = await GetDrinkByIdAsync(secondId);

            // Проверяем, что оба напитка существуют
            if (firstDrink != null && secondDrink != null)
            {
                // Сохраняем старые значения
                var tempName = firstDrink.name;
                var tempPrice = firstDrink.price;
                var tempQuantity = firstDrink.quantity;
                var tempBrandId = firstDrink.brandId;
                var tempBrand = firstDrink.brand; // Если brand - это объект, необходимо убедиться, что он копируется
                var tempImagePath = firstDrink.imagePath;

                Console.WriteLine($"{firstDrink.name}  {secondDrink.name}");
                Console.WriteLine($"{firstDrink.price}  {secondDrink.price}");
                Console.WriteLine($"{firstDrink.quantity}  {secondDrink.quantity}");
                Console.WriteLine($"{firstDrink.brandId}  {secondDrink.brandId}");
                Console.WriteLine($"{firstDrink.imagePath}  {secondDrink.imagePath}");

                // Обмен значениями
                firstDrink.name = secondDrink.name;
                firstDrink.price = secondDrink.price;
                firstDrink.quantity = secondDrink.quantity;
                firstDrink.brandId = secondDrink.brandId;
                firstDrink.brand = secondDrink.brand; // Может потребоваться глубокое копирование
                firstDrink.imagePath = secondDrink.imagePath;

                secondDrink.name = tempName;
                secondDrink.price = tempPrice;
                secondDrink.quantity = tempQuantity;
                secondDrink.brandId = tempBrandId;
                secondDrink.brand = tempBrand; // Может потребоваться глубокое копирование
                secondDrink.imagePath = tempImagePath;

                // Логирование для отладки
                Console.WriteLine($"{firstDrink.name}  {secondDrink.name}");
                Console.WriteLine($"{firstDrink.price}  {secondDrink.price}");
                Console.WriteLine($"{firstDrink.quantity}  {secondDrink.quantity}");
                Console.WriteLine($"{firstDrink.brandId}  {secondDrink.brandId}");
                Console.WriteLine($"{firstDrink.imagePath}  {secondDrink.imagePath}");

                // Сохраняем изменения в базе данных
                await SaveChangesAsync();
                return true;
            }
            return false;
        }



        // Сохранить изменения в базе данных
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
