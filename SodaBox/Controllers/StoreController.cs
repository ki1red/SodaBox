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
using SodaBox.Services.Interfaces;
using SodaBox.Services.Classes;

public class StoreController : Controller
{
    private readonly VendingMachineContext _context;
    private readonly ICartService _cartService;
    private readonly ITransactionService _transactionService;

    public StoreController(VendingMachineContext context, ICartService cartService, ITransactionService transactionService)
    {
        _context = context;
        _cartService = cartService;
        _transactionService = transactionService;
    }

    // Главная страница магазина
    public async Task<IActionResult> Index()
    {
        _transactionService.ResetTransaction();

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
    [HttpPut]
    public IActionResult AddOrRemoveToCart(int drinkId)
    {
        var drink = _context.drinks.Find(drinkId);
        if (drink == null)
        {
            Console.WriteLine("Напиток не найден");
            return NotFound();
        }

        if (!_cartService.IsHasCart(drink))
        {
            Console.WriteLine($"Добавляем напиток в корзину: {drink.name}");
            _cartService.AddToCart(drink);
        }
        else
        {
            Console.WriteLine($"Удаляем напиток из корзины: {drink.name}");
            _cartService.RemoveFromCart(drink);
        }

        return Ok();
    }

    [HttpGet]
    public IActionResult GetCartJson()
    {
        var cart = _cartService.GetCart();
        return Json(cart);
    }

}
