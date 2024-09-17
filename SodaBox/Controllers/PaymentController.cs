using Microsoft.AspNetCore.Http;
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
        else if (_transactionService.IsTransactionCompleted() &&
            _transactionService.completeSum != null)
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

    [HttpPost]
    public IActionResult Pay([FromBody] int sum)
    {
        _transactionService.CompleteTransaction(sum);
        return Ok(new { redirectUrl = Url.Action("Change", "Payment") });
    }

    public IActionResult Change()
    {
        if (!_transactionService.IsTransactionCompleted())
        {
            return RedirectToAction("Index", "Store");
        }

        Response.Headers.Append("Cache-Control", "no-store");
        Response.Headers.Append("Pragma", "no-cache");
        Response.Headers.Append("Expires", "0");

        var viewModel = new ChangeViewModel
        {
            totalAmount = _transactionService.requestSum.Value,
            currentAmount = _transactionService.completeSum.Value
        };
        return View(viewModel);
    }
}
