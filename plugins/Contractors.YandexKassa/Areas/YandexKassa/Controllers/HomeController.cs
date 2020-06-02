using Contractors.YandexKassa.Areas.YandexKassa.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Contractors.YandexKassa.Areas.YandexKassa.Controllers
{
    [Area("YandexKassa")]
    public class HomeController : Controller
    {
        [HttpPost]
        public IActionResult Start(int orderId, string description, decimal amount)
        {
            var model = new StartModel
            {
                OrderId = orderId,
                Description = description,
                Amount = amount
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Card(int orderId, string description, decimal amount)
        {
            return RedirectToAction(nameof(Callback), new { orderId, amount });
        }

        [HttpGet]
        public IActionResult Callback(int orderId, decimal amount)
        {
            var model = new StartModel
            {
                OrderId = orderId,
                Amount = amount
            };

            return View(model);
        }
    }
}
