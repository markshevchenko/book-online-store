using Microsoft.AspNetCore.Mvc;
using Store;
using Web.Models;

namespace Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IBookRepository bookRepository;

        public CartController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(int id)
        {
            var book = bookRepository.GetById(id);
            Cart cart;
            if (!HttpContext.Session.TryGetCart("Cart", out cart))
                cart = new Cart();

            if (cart.Items.ContainsKey(id))
            {
                cart.Items[id]++;
                cart.Amount += book.Price;
            }
            else
            {
                cart.Items[id] = 1;
                cart.Amount = book.Price;
            }

            HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("Index", "Book", new { id });
        }
    }
}