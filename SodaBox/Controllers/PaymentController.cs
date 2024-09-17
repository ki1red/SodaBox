using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SodaBox.Models;
using System.Collections.Generic;
using System.Linq;

public class PaymentController : Controller
{
    public IActionResult Payment(int sum)
    {

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
            return RedirectToAction("Success");
        }
        return View("Payment", model);
    }

    public IActionResult Success()
    {
        return View("Success");
    }
}
