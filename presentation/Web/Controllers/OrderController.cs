using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store;
using Web.Models;

namespace Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IBookRepository bookRepository;
        private readonly INotificationService notificationService;

        public OrderController(IOrderRepository orderRepository,
                              IBookRepository bookRepository,
                              INotificationService notificationService)
        {
            this.orderRepository = orderRepository;
            this.bookRepository = bookRepository;
            this.notificationService = notificationService;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                var order = orderRepository.GetById(cart.OrderId);
                var model = Map(order);
                
                return View(model);
            }

            return View("Empty");
        }

        private OrderModel Map(Order order)
        {
            var bookIds = order.Items.Select(item => item.BookId);
            var books = bookRepository.GetAllByIds(bookIds);
            var itemModels = from item in order.Items
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
                State = order.State,
                Items = itemModels.ToArray(),
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

            orderRepository.Update(order);
            HttpContext.Session.Set(cart);

            return RedirectToAction("Index", "Book", new { id });
        }

        [HttpPost]
        public IActionResult StartProcess(int id)
        {
            var order = orderRepository.GetById(id);
            order.StartProcess();
            orderRepository.Update(order);

            var model = Map(order);

            return View(model);
        }

        [HttpPost]
        public IActionResult SendConfirmation(int id, string cellPhone)
        {
            var order = orderRepository.GetById(id);
            var model = Map(order);

            if (!IsValidCellPhone(cellPhone))
            {
                model.Errors["cellPhone"] = "Пустой или не соответствует формату +79876543210";
                return View("StartProcess", model);
            }

            var code = GenerateCode();
            HttpContext.Session.SetInt32(cellPhone, code);
            notificationService.SendConfirmationCode(cellPhone, code);
            model.CellPhone = cellPhone;

            return View(model);
        }

        private bool IsValidCellPhone(string cellPhone)
        {
            cellPhone = cellPhone?.Replace(" ", "")
                                 ?.Replace("-", "");

            return Regex.IsMatch(cellPhone, @"^\+?\d{11}$");
        }

        private int GenerateCode()
        {
            var random = new Random();
            return random.Next(1, 10000);
        }

        [HttpPost]
        public IActionResult ConfirmCellPhone(int id, string cellPhone, int code)
        {
            var order = orderRepository.GetById(id);
            var model = Map(order);

            var storedCode = HttpContext.Session.GetInt32(cellPhone);
            if (storedCode == null)
                return View("StartProcess", model);

            if (storedCode != code)
            {
                model.Errors["code"] = "Отличается от отправленного";
                return View("SendConfirmation", model);
            }

            order.CellPhone = cellPhone;
            orderRepository.Update(order);

            HttpContext.Session.Remove(cellPhone);

            return View(model);
        }
    }
}