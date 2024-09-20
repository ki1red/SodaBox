using SodaBox.DataAccess.Entities;

namespace SodaBox.DataAccess.IRepositories
{
    public interface ICoinRepository
    {
        // Получить один элемент по номиналу
        Task<Coin> GetElementByPriceAsync(int price);
        // Получить все элементы
        Task<IEnumerable<Coin>> GetAllElementsAsync();
        // Добавить монет
        Task<bool> AddCoinsAsync(int price, int count);
        // Получить указанную сумму монетами
        Task<IEnumerable<Coin>> TakeCoinsAsync(int sum);
        Task SaveChangesAsync();
    }
}
