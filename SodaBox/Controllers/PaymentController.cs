using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SodaBox.DataAccess.IRepositories;
using SodaBox.Models;
using SodaBox.Services.Classes;
using SodaBox.Services.Interfaces;
using System.Net.Http;

public class PaymentController : Controller
{
    private readonly ITransactionService _transactionService;
    private readonly ICoinRepository _coinRepository;
    public PaymentController(ITransactionService transactionService, ICoinRepository coinRepository)
    {
        _transactionService = transactionService;
        _coinRepository = coinRepository;
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
        if (!_transactionService.IsTransactionCompleted())
        {
            return RedirectToAction("Index", "Store");
        }

        Response.Headers.Append("Cache-Control", "no-store");
        Response.Headers.Append("Pragma", "no-cache");
        Response.Headers.Append("Expires", "0");

        var coins = (await _coinRepository.TakeCoinsAsync(_transactionService.completeSum.Value - _transactionService.requestSum.Value)).ToList();
        if (coins == null || coins.Count == 0)
        {
            return View("NoChange");
        }

        var viewModel = new ChangeViewModel
        {
            totalAmount = _transactionService.requestSum.Value,
            currentAmount = _transactionService.completeSum.Value,
            coins = coins
        };
        return View(viewModel);
    }
}
