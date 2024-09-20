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
            throw new NotImplementedException();
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
