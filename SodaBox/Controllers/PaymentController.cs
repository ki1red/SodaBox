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
    public IActionResult Payment()
    {
        // Если оплата не начата, то страница оплаты не может быть доступна
        if (!_transactionService.IsTransactionCompleted())
        {
            return RedirectToAction("Bucket", "Bucket");
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

    [HttpPost]
    public IActionResult Pay(PaymentViewModel model)
    {
        if (model.isCanPay)
        {
            // Логика обработки успешной оплаты
            _transactionService.CompleteTransaction(model.currentAmount);
            return RedirectToAction("Change");
        }
        return View("Payment", model);
    }

}
