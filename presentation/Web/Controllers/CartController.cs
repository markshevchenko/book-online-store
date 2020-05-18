using Microsoft.AspNetCore.Mvc;
using Store;
using Web.Models;

namespace Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IBookRepository bookRepository;

        public CartController(IOrderRepository orderRepository,
                              IBookRepository bookRepository)
        {
            this.orderRepository = orderRepository;
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
            Order order;
            Cart cart;

            if (HttpContext.Session.TryGetCart(out cart))
            {
                order = orderRepository.GetById(cart.OrderId);
            }
            else
            {
                order = orderRepository.Create();
                cart = new Cart(order.Id);
            }

            order.AddItem(book, 1);
            cart.TotalCount = order.TotalCount;
            cart.TotalAmount = order.TotalAmount;

            HttpContext.Session.Set(cart);

            return RedirectToAction("Index", "Book", new { id });
        }
    }
}