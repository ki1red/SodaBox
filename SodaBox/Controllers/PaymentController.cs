using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SodaBox.DataAccess.IRepositories;
using SodaBox.DataAccess.Repositories;
using SodaBox.Models;
using SodaBox.Services.Classes;
using SodaBox.Services.Interfaces;
using System.Net.Http;

public class PaymentController : Controller
{
    private readonly ITransactionService _transactionService;
    private readonly ICoinRepository _coinRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IDrinkRepository _drinkRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly ICartService _cartService;
    public PaymentController(ITransactionService transactionService, ICoinRepository coinRepository, ICartService cartService, IOrderRepository orderRepository, IDrinkRepository drinkRepository, IBrandRepository brandRepository)
    {
        _transactionService = transactionService;
        _coinRepository = coinRepository;
        _orderRepository = orderRepository;
        _drinkRepository = drinkRepository;
        _brandRepository = brandRepository;
        _cartService = cartService;
    }
    public IActionResult Payment()
    {
        // Если оплата не начата, то страница оплаты не может быть доступна
        if (!_transactionService.isStart)
        {
            return RedirectToAction("Bucket", "Bucket");
        }
        else if (_transactionService.isComplete)
        {
            return RedirectToAction("Change", "Payment");
        }

        Response.Headers.Append("Cache-Control", "no-store");
        Response.Headers.Append("Pragma", "no-cache");
        Response.Headers.Append("Expires", "0");
        var viewModel = new PaymentViewModel
        {
            totalAmount = _transactionService.requestSum.Value
        };

        return View(viewModel);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCoinQuantity([FromBody] Dictionary<int,int> coins)
    {
        if (coins == null)
            return BadRequest();

        foreach (var coin in coins)
        {
            bool response = await _coinRepository.AddCoinsAsync(coin.Key, coin.Value);
            return BadRequest(response);
        }

        return Ok();
    }

    [HttpPost]
    public ActionResult Pay([FromBody] int sum)
    {
        _transactionService.CompleteTransaction(sum);

        return Ok(new { redirectUrl = Url.Action("Change", "Payment") });
    }

    public async Task<IActionResult> Change()
    {
        if (!_transactionService.isStart)
        {
            return RedirectToAction("Index", "Store");
        }

        Response.Headers.Append("Cache-Control", "no-store");
        Response.Headers.Append("Pragma", "no-cache");
        Response.Headers.Append("Expires", "0");


        var coins = (await _coinRepository.TakeCoinsAsync(_transactionService.completeSum.Value - _transactionService.requestSum.Value)).ToList();
        foreach (var coin in coins)
            Console.WriteLine($"price={coin.price} quantity={coin.quantity}");
        if (coins == null || coins.Count == 0)
        {
            return View("NoChange");
        }

        List<(string brandName, string drinkName, int quantity, int price)> orderItems = new List<(string brandName, string drinkName, int quantity, int price)>();
        var cart = _cartService.GetCart();
        foreach (var item in cart)
        {
            string brandName = (await _brandRepository.GetBrandByIdAsync(item.drink.brandId)).name;
            orderItems.Add((brandName, item.drink.name, item.quantity, item.drink.price));
            await _drinkRepository.UpdateQuantityAsync(item.drink.id, item.drink.quantity - item.quantity);
        }
        await _orderRepository.AddOrderAsync(DateTime.Now, _transactionService.requestSum.Value, orderItems);
        _cartService.ClearCart();

        var viewModel = new ChangeViewModel
        {
            totalAmount = _transactionService.requestSum.Value,
            currentAmount = _transactionService.completeSum.Value,
            coins = coins
        };
        return View(viewModel);
    }
}
