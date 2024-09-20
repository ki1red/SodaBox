using SodaBox.DataAccess.Entities;

namespace SodaBox.DataAccess.IRepositories
{
    public interface IDrinkRepository
    {
        Task<Drink> GetDrinkByIdAsync(int id);
        Task<IEnumerable<Drink>> GetAllDrinksAsync();
        Task<IEnumerable<Drink>> SortByDrinksAsync(int brandId, int minPrice, int maxPrice);
        Task<bool> UpdatePriceAsync(int drinkId, int newPrice);
        Task<bool> UpdateQuantityAsync(int drinkId, int newQuantity);
        Task<bool> SwapPositionsAsync(int oldId, int newId);
        Task SaveChangesAsync();
    }
}
