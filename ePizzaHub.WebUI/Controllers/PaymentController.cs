using ePizzaHub.Repositories.Models;
using ePizzaHub.WebUI.Helpers;
using ePizzaHub.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ePizzaHub.WebUI.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            PaymentModel paymentModel = new PaymentModel();
            CartModel cart = TempData.Peek<CartModel>("Cart"); 
            if(cart != null)
            {
                paymentModel.Cart = cart;
            }
            paymentModel.GrandTotal=Math.Round(cart.GrandTotal);
            paymentModel.Currency = "INR";

            string items = "";
            foreach (var item in cart.Items)
            {
                items += item.Name + ",";
            }
            paymentModel.Description= items;
            return View(paymentModel);
        }
    }
}
