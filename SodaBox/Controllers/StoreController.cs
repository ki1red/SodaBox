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
using SodaBox.DataAccess.IRepositories; // Добавляем интерфейс репозитория

public class StoreController : Controller
{
    private readonly ICartService _cartService;
    private readonly ITransactionService _transactionService;
    private readonly IDrinkRepository _drinkRepository;
    private readonly IBrandRepository _brandRepository;

    // Изменяем конструктор, чтобы получать репозиторий через DI
    public StoreController(ICartService cartService, ITransactionService transactionService, IDrinkRepository drinkRepository, IBrandRepository brandRepository)
    {
        _cartService = cartService;
        _transactionService = transactionService;
        _drinkRepository = drinkRepository;
        _brandRepository = brandRepository;
    }

    // Главная страница магазина
    public async Task<IActionResult> Index()
    {
        //if (_transactionService.IsTransactionCompleted() &&
        //    _transactionService.requestSum != null &&
        //    _transactionService.completeSum != null)
        //{
        //    Response.Headers.Append("Cache-Control", "no-store");
        //    Response.Headers.Append("Pragma", "no-cache");
        //    Response.Headers.Append("Expires", "0");

        //    //UpdateDrinksStock();
        //    // TODO нужно сделать обновление количества напитков в БД после покупки
        //}

        _transactionService.RemoveTransaction();

        // Используем репозиторий для получения списка брендов и напитков
        var brands = await _brandRepository.GetAllBrandsAsync();
        var drinks = await _drinkRepository.GetAllDrinksAsync();
        var viewModel = new StoreViewModel
        {
            brands = brands.ToList(),
            drinks = drinks.ToList()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Admin()
    {
        _transactionService.RemoveTransaction();

        var drinks = await _drinkRepository.GetAllDrinksAsync();
        var viewModel = new AdminViewModel
        {
            drinks = drinks.ToList()
        };

        return View(viewModel);
    }

    // API для фильтрации
    [HttpGet]
    public async Task<IActionResult> FilterDrinks(int brandId, int minPrice, int maxPrice)
    {

        var drinks = await _drinkRepository.SortByDrinksAsync(brandId, minPrice, maxPrice);

        return PartialView("_DrinkListPartial", drinks);
    }

    // Добавление напитка в корзину
    [HttpPut]
    public async Task<IActionResult> AddOrRemoveToCart(int drinkId)
    {
        var drink = await _drinkRepository.GetDrinkByIdAsync(drinkId); // Используем репозиторий

        if (drink == null)
            return NotFound();

        if (!_cartService.IsHasCart(drink))
            _cartService.AddToCart(drink);
        else
            _cartService.RemoveFromCart(drink);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDrinksStock([FromBody] Dictionary<int, int>? drinks = null)
    {
        if (drinks != null)
        {
            foreach (var drink in drinks)
            {
                bool response = await _drinkRepository.UpdateQuantityAsync(drink.Key, drink.Value);
                if (!response)
                    return BadRequest();
            }
        }
        else
            return BadRequest();

        _cartService.ClearCart();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDrinksPrice([FromBody] Dictionary<int, int>? drinks = null)
    {
        if (drinks != null)
        {
            foreach (var drink in drinks)
            {
                bool response = await _drinkRepository.UpdatePriceAsync(drink.Key, drink.Value);
                if (!response)
                    return BadRequest();
            }
        }
        else
            return BadRequest();

        _cartService.ClearCart();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDrinksPositions([FromBody] List<int>? drinks = null)
    {
        if (drinks != null && drinks.Count > 0)
        {
            Dictionary<int, int> positions = drinks
                .Select((value, index) => new { Index = index + 1, Value = value }) // Индексация с 1
                .ToDictionary(item => item.Index, item => item.Value);

            var response = await _drinkRepository.UpdateAllDrinksPositions(positions);
            if (!response)
                return BadRequest();

            Console.WriteLine("Напитки обновлены");
        }
        _cartService.ClearCart();
        return Ok();
    }

    [HttpGet]
    public IActionResult GetCartJson()
    {
        var cart = _cartService.GetCart();
        return Json(cart);
    }
}
