using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SodaBox.DataAccess;
using SodaBox.Models;
using SodaBox;
using SodaBox.DataAccess.Entities;

public class StoreController : Controller
{
    private readonly VendingMachineContext _context;

    public StoreController(VendingMachineContext context)
    {
        _context = context;
    }

    // Главная страница магазина
    public async Task<IActionResult> Index()
    {
        var brands = await _context.brands.ToListAsync();
        var drinks = await _context.drinks.Include(d => d.brand).ToListAsync();

        var viewModel = new StoreViewModel
        {
            brands = brands,
            drinks = drinks // Возвращаем простой список напитков
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

        return PartialView("_DrinkListPartial", drinks); // Передаем обычный список напитков
    }


}
