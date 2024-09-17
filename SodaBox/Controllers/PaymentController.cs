using Microsoft.AspNetCore.Mvc;
using SodaBox.Models;
using SodaBox.Services.Classes;
using SodaBox.Services.Interfaces;

public class PaymentController : Controller
{
    private readonly ITransactionService _transactionService;
    public PaymentController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }
    public IActionResult Payment(int sum)
    {
        // Если оплата проведена, то страница оплаты не может быть доступна
        if (_transactionService.IsTransactionCompleted())
        {
            return RedirectToAction("Index", "Store");
        }

        Response.Headers.Append("Cache-Control", "no-store");
        Response.Headers.Append("Pragma", "no-cache");
        Response.Headers.Append("Expires", "0");

        var viewModel = new PaymentViewModel
        {
            totalAmount = sum
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Pay(PaymentViewModel model)
    {
        if (model.isCanPay)
        {
            // Логика обработки успешной оплаты
            return RedirectToAction("Change", new { amount = model.currentAmount - model.totalAmount });
        }
        return View("Payment", model);
    }

    public IActionResult Change(int amount)
    {
        _transactionService.CompleteTransaction();
        ViewBag.changeAmount = amount;
        return View();
    }

    public IActionResult Success()
    {
        return View("Success");
    }
}
