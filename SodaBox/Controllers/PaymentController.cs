using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SodaBox.Models;
using System.Collections.Generic;
using System.Linq;

public class PaymentController : Controller
{
    [HttpGet]
    public IActionResult Payment()
    {
        var totalAmount = HttpContext.Session.GetDecimal("TotalAmount");

        var viewModel = new PaymentViewModel
        {
            TotalAmount = totalAmount
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult ProcessPayment(Dictionary<int, int> coins)
    {
        var totalAmount = HttpContext.Session.GetDecimal("TotalAmount");
        var currentAmount = coins.Sum(c => c.Key * c.Value);
        var change = currentAmount - totalAmount;

        if (change < 0)
        {
            // Недостаточно средств
            TempData["Error"] = "Недостаточно средств для оплаты.";
            return RedirectToAction("Payment");
        }

        // Логика обработки платежа
        HttpContext.Session.Set("Change", change);

        return RedirectToAction("Change");
    }

    [HttpGet]
    public IActionResult Change()
    {
        var change = HttpContext.Session.Get<decimal>("Change");

        // Логика для вычисления сдачи
        var coins = CalculateChange(change);

        var viewModel = new ChangeViewModel
        {
            Change = change,
            Coins = coins
        };

        return View(viewModel);
    }

    private Dictionary<int, int> CalculateChange(decimal amount)
    {
        var coins = new Dictionary<int, int>
        {
            { 10, 0 },
            { 5, 0 },
            { 2, 0 },
            { 1, 0 }
        };

        foreach (var coin in coins.Keys.OrderByDescending(c => c))
        {
            while (amount >= coin)
            {
                coins[coin]++;
                amount -= coin;
            }
        }

        return coins;
    }
}
