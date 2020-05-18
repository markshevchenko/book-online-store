using Microsoft.AspNetCore.Mvc;
using Store;
using System.Linq;
using Web.Models;

namespace Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IBookRepository bookRepository;

        public OrderController(IOrderRepository orderRepository,
                              IBookRepository bookRepository)
        {
            this.orderRepository = orderRepository;
            this.bookRepository = bookRepository;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                var order = orderRepository.GetById(cart.OrderId);
                var bookIds = order.Items.Select(item => item.BookId);
                var books = bookRepository.GetAllByIds(bookIds);

                var model = CreateOrderModel(order, books);
                
                return View(model);
            }

            return View("Empty");
        }

        private OrderModel CreateOrderModel(Order order, Book[] books)
        {
            var orderItemModels = from item in order.Items
                                  join book in books on item.BookId equals book.Id
                                  select new OrderItemModel
                                  {
                                      BookId = book.Id,
                                      Title = book.Title,
                                      Author = book.Author,
                                      Price = item.Price,
                                      Count = item.Count,
                                  };

            return new OrderModel
            {
                Id = order.Id,
                Items = orderItemModels.ToArray(),
                TotalCount = order.TotalCount,
                TotalAmount = order.TotalAmount,
            };
        }

        [HttpPost]
        public IActionResult AddItem(int id)
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