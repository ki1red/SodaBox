using Microsoft.EntityFrameworkCore;
using SodaBox.DataAccess.Entities;
using SodaBox.DataAccess.IRepositories;

namespace SodaBox.DataAccess.Repositories
{
    public class CoinRepository : ICoinRepository
    {
        private readonly VendingMachineContext _context;

        public CoinRepository(VendingMachineContext context)
        {
            _context = context;
        }

        public async Task<bool> AddCoinsAsync(int price, int count)
        {
            var coin = await GetElementByPriceAsync(price);

            if (coin != null && count > 0)
            {
                coin.quantity += count;
                await SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Coin>> GetAllElementsAsync()
        {
            return await _context.coins.ToListAsync();
        }

        public async Task<IEnumerable<Coin>> TakeCoinsAsync(int sum)
        {
            // Получаем все монеты с положительным количеством и сортируем по убыванию номинала
            var availableCoins = await _context.coins
                .Where(c => c.quantity > 0)
                .OrderByDescending(c => c.price)
                .ToListAsync();

            var remainingAmount = sum;
            var coinsToReturn = new List<Coin>();

            // Создаем копию доступных монет для проверки, но не будем изменять БД, пока не убедимся, что можем выдать сдачу
            var coinsSnapshot = availableCoins.Select(c => new Coin
            {
                id = c.id,
                price = c.price,
                quantity = c.quantity
            }).ToList();

            foreach (var coin in coinsSnapshot)
            {
                if (remainingAmount <= 0)
                {
                    break;
                }

                // Определяем, сколько монет текущего номинала нам нужно
                int neededCoins = remainingAmount / coin.price;

                if (neededCoins > 0)
                {
                    // Берем либо столько монет, сколько нужно, либо сколько есть в наличии
                    int coinsToTake = Math.Min(neededCoins, coin.quantity);

                    // Если мы берем хотя бы одну монету
                    if (coinsToTake > 0)
                    {
                        remainingAmount -= coinsToTake * coin.price;

                        // Обновляем количество монет в копии списка
                        coin.quantity -= coinsToTake;

                        // Добавляем монеты в список тех, что мы собираемся вернуть
                        coinsToReturn.Add(new Coin
                        {
                            id = coin.id,
                            price = coin.price,
                            quantity = coinsToTake
                        });
                    }
                }
            }

            // Если после попытки собрать сдачу у нас осталась непокрытая сумма
            if (remainingAmount > 0)
            {
                // Невозможно выдать сдачу, возвращаем null
                return null;
            }

            // Если сдачу можно выдать, обновляем реальные данные в БД
            foreach (var coin in coinsToReturn)
            {
                var coinInDb = await _context.coins.FindAsync(coin.id);
                if (coinInDb != null)
                {
                    coinInDb.quantity -= coin.quantity;
                }
            }

            // Сохраняем изменения в БД
            await _context.SaveChangesAsync();
            foreach (var coin in coinsToReturn)
            {
                Console.WriteLine($"{coin.id} {coin.price} {coin.quantity}");
            }
            // Возвращаем список монет, которые выдаем в качестве сдачи
            return coinsToReturn;
        }


        public async Task<Coin> GetElementByPriceAsync(int price)
        {
            return await _context.coins.FirstOrDefaultAsync(c => c.price == price);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
