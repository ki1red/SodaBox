using Microsoft.EntityFrameworkCore;
using SodaBox.DataAccess.Entities;
using SodaBox.Models;
using SodaBox.Services.Interfaces;
using System.Text.Json;

namespace SodaBox.Services.Classes
{
    public class CartService : ICartService
    {
        private readonly string _cartSessionKey;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartService(IHttpContextAccessor? httpContextAccessor = null, string? cartSessionKey = null)
        {
            if (httpContextAccessor == null)
                _httpContextAccessor = new HttpContextAccessor();
            else
                _httpContextAccessor = httpContextAccessor;

            if (cartSessionKey == null)
                _cartSessionKey = Guid.NewGuid().ToString();
            else
                _cartSessionKey = cartSessionKey;
        }
        public List<CartItem> GetCart()
        {
            var cartJson = _httpContextAccessor.HttpContext.Session.GetString(_cartSessionKey);
            return cartJson == null ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(cartJson);
        }
        
        public void SaveCart(List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            _httpContextAccessor.HttpContext.Session.SetString(_cartSessionKey, cartJson);
        }
        public bool AddToCart(Drink drink)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(item => item.drink.id == drink.id);
            
            if (cartItem != null)
                return false;

            cart.Add(new CartItem { drink = drink, quantity = 1 });

            SaveCart(cart);

            return true;
        }

        public bool RemoveFromCart(Drink drink)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(item => item.drink.id == drink.id);

            if (cartItem == null)
                return false;

            if (!cart.Remove(cartItem))
                return false;

            SaveCart(cart);

            return true;
        }

        public bool IsHasCart(Drink drink)
        {
            var cart = GetCart();
            if (cart.FirstOrDefault(item => item.drink.id == drink.id) == null)
                return false;
            else 
                return true;
        }

        public bool UpdateCart(int drinkId, int newQuantity)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(item => item.drink.id == drinkId);

            if (cartItem == null)
                return false;
            
            cartItem.quantity = newQuantity;

            SaveCart(cart);

            return true;
        }
    }

    public class CartItem
    {
        public Drink drink { get; set; }
        public int quantity { get; set; }
    }
}
