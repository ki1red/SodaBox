using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SodaBox.DataAccess;
using SodaBox.Models;
using SodaBox.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

public class StoreController : Controller
{
    private readonly VendingMachineContext _context;
    private const string CartSessionKey = "Cart";

    public StoreController(VendingMachineContext context)
    {
        _context = context;
    }

    // Главная страница магазина
    public async Task<IActionResult> Index()
    {
        var brands = await _context.brands.ToListAsync();
        var drinks = await _context.drinks.Include(d => d.brand).ToListAsync(); // Получаем все напитки

        var viewModel = new StoreViewModel
        {
            brands = brands,
            drinks = drinks
        };

        return View(viewModel);
    }

    // API для фильтрации
    [HttpGet]
    public async Task<IActionResult> FilterDrinks(int? brandId, decimal? minPrice, decimal? maxPrice)
    {
        var drinksQuery = _context.drinks.Include(d => d.brand).AsQueryable();

        if (brandId.HasValue && brandId.Value > 0)
        {
            drinksQuery = drinksQuery.Where(d => d.brandId == brandId.Value);
        }

        if (minPrice.HasValue)
        {
            drinksQuery = drinksQuery.Where(d => d.price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            drinksQuery = drinksQuery.Where(d => d.price <= maxPrice.Value);
        }

        var drinks = await drinksQuery.ToListAsync();

        return PartialView("_DrinkListPartial", drinks);
    }

    // Добавление напитка в корзину
    [HttpPost]
    public IActionResult AddToCart(int drinkId)
    {
        var drink = _context.drinks.Find(drinkId);
        if (drink == null)
        {
            Console.WriteLine("Напиток не найден");
            return NotFound();
        }

        var cart = GetCart();

        var cartItem = cart.FirstOrDefault(item => item.drink.id == drinkId);
        if (cartItem == null)
        {
            Console.WriteLine($"Добавляем напиток в корзину: {drink.name}");
            cart.Add(new CartItem { drink = drink, quantity = 1 });
        }

        SaveCart(cart);

        return Ok();
    }

    // Страница корзины
    public IActionResult Bucket()
    {
        var cart = GetCart();
        var viewModel = new BucketViewModel
        {
            cartItems = cart
        };

        return View(viewModel);
    }

    // API для обновления количества напитков в корзине
    [HttpPost]
    public IActionResult UpdateCart(int drinkId, int quantity)
    {
        var cart = GetCart();

        var cartItem = cart.FirstOrDefault(item => item.drink.id == drinkId);
        if (cartItem != null)
        {
            if (quantity < 1)
            {
                cart.Remove(cartItem);
            }
            else
            {
                cartItem.quantity = quantity;
            }
        }

        SaveCart(cart);

        return RedirectToAction("Bucket");
    }

    [HttpGet]
    public IActionResult GetCartJson()
    {
        var cart = GetCart();
        return Json(cart);
    }

    private List<CartItem> GetCart()
    {
        var cartJson = HttpContext.Session.GetString(CartSessionKey);
        return cartJson == null ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(cartJson);
    }


    private void SaveCart(List<CartItem> cart)
    {
        var cartJson = JsonSerializer.Serialize(cart);
        HttpContext.Session.SetString(CartSessionKey, cartJson);
    }
}
