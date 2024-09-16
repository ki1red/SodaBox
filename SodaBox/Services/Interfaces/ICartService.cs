using SodaBox.DataAccess.Entities;
using SodaBox.Models;
using SodaBox.Services.Classes;

namespace SodaBox.Services.Interfaces
{
    public interface ICartService
    {
        List<CartItem> GetCart();

        void SaveCart(List<CartItem> cart);
        bool AddToCart(Drink drink);
        bool RemoveFromCart(Drink drink);
        bool IsHasCart(Drink drink);
        bool UpdateCart(int drinkId, int newQuantity);
    }
}
