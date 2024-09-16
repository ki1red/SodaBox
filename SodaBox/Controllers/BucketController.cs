using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SodaBox.DataAccess;
using SodaBox.Services.Interfaces;
using SodaBox.Models;

namespace SodaBox.Controllers
{
    public class BucketController : Controller
    {
        private readonly ICartService _cartService;
        public BucketController(ICartService cartService)
        {
            _cartService = cartService;
        }
        // Страница корзины
        public IActionResult Bucket()
        {
            var cart = _cartService.GetCart();
            var viewModel = new BucketViewModel
            {
                cartItems = cart,
                sum = cart.Sum(item => item.drink.price * item.quantity)
            };

            return View(viewModel);
        }

        //[HttpPost]
        //public IActionResult GoToPayment()
        //{
        //    var cart = GetCart();
        //    var totalAmount = cart.Sum(item => item.drink.price * item.quantity);

        //    HttpContext.Session.SetDecimal("TotalAmount", totalAmount);

        //    return RedirectToAction("Payment", "Payment");
        //}

        // API для обновления количества напитков в корзине
        [HttpPut]
        public IActionResult ReloadCart(int drinkId, int quantity)
        {
            _cartService.UpdateCart(drinkId, quantity);

            return Ok();
        }
    }
}
