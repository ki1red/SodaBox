using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SodaBox.DataAccess;
using SodaBox.Services.Interfaces;
using SodaBox.Models;
using System;

namespace SodaBox.Controllers
{
    public class BucketController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ITransactionService _transactionService;
        public BucketController(ICartService cartService, ITransactionService transactionService)
        {
            _cartService = cartService;
            _transactionService = transactionService;
        }
        // Страница корзины
        public IActionResult Bucket()
        {
            // Если оплата проводится, корзина не может быть доступна
            if (_transactionService.IsTransactionCompleted())
            {
                if (_transactionService.completeSum != null)
                    return RedirectToAction("Index", "Store");
            }

            Response.Headers.Append("Cache-Control", "no-store");
            Response.Headers.Append("Pragma", "no-cache");
            Response.Headers.Append("Expires", "0");

            var cart = _cartService.GetCart();
            var viewModel = new BucketViewModel
            {
                cartItems = cart,
                sum = cart.Sum(item => item.drink.price * item.quantity)
            };

            return View(viewModel);
        }

        
        [HttpPut]
        public IActionResult ReloadCart(int drinkId, int quantity)
        {
            _cartService.UpdateCart(drinkId, quantity);

            return Ok();
        }

        [HttpPost]
        public IActionResult Payment([FromBody] int sum)
        {
            if (sum != _cartService.GetCart().Sum(item => item.drink.price * item.quantity))
            {
                return BadRequest("Invalid data");
            }

            _transactionService.StartTransaction(sum);

            return Ok(new { redirectUrl = Url.Action("Payment", "Payment") });
        }
        [HttpPut]
        public IActionResult DeleteItem(int drinkId)
        {
            var cart = _cartService.GetCart();
            var cartItem = cart.FirstOrDefault(item => item.drink.id == drinkId);
            _cartService.RemoveFromCart(cartItem.drink);
            return Ok();
        }

    }
}
