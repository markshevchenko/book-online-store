using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store;
using Store.Contractors;
using Store.Messages;
using Web.Contractors;
using Web.Models;

namespace Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IBookRepository bookRepository;
        private readonly IEnumerable<IDeliveryService> deliveryServices;
        private readonly IEnumerable<IPaymentService> paymentServices;
        private readonly IEnumerable<IWebService> webServices;
        private readonly INotificationService notificationService;

        public OrderController(IOrderRepository orderRepository,
                               IBookRepository bookRepository,
                               IEnumerable<IDeliveryService> deliveryServices,
                               IEnumerable<IPaymentService> paymentServices,
                               IEnumerable<IWebService> webServices,
                               INotificationService notificationService)
        {
            this.orderRepository = orderRepository;
            this.bookRepository = bookRepository;
            this.deliveryServices = deliveryServices;
            this.paymentServices = paymentServices;
            this.webServices = webServices;
            this.notificationService = notificationService;
        }

        [HttpGet]
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
                DeliveryDescription = order.Delivery?.Description,
            };
        }

        [HttpPost]
        public IActionResult AddItem(int bookId)
        {
            var book = bookRepository.GetById(bookId);

            (Order order, Cart cart) = GetCurrentOrderAndCart();

            order.Items.Add(book.Id, book.Price, 1);
            cart.TotalCount = order.TotalCount;
            cart.TotalAmount = order.TotalAmount;

            orderRepository.Update(order);
            HttpContext.Session.Set(cart);

            return RedirectToAction("Index", "Book", new { id = bookId });
        }

        private (Order order, Cart cart) GetCurrentOrderAndCart()
        {
            Order order;

            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                order = orderRepository.GetById(cart.OrderId);

                return (order, cart);
            }

            order = orderRepository.Create();
            cart = new Cart(order.Id);

            return (order, cart);
        }

        [HttpPost]
        public IActionResult SetCellPhone(int id)
        {
            var order = orderRepository.GetById(id);

            var model = Map(order);

            return View("CellPhone", model);
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

            return View("Confirmation", model);
        }

        private bool IsValidCellPhone(string cellPhone)
        {
            cellPhone = cellPhone?.Replace(" ", "")
                                 ?.Replace("-", "");

            return Regex.IsMatch(cellPhone, @"^\+?\d{11}$");
        }

        private int GenerateCode()
        {
            return 1111;
        }

        [HttpPost]
        public IActionResult ConfirmCellPhone(int id, string cellPhone, int code)
        {
            var order = orderRepository.GetById(id);
            var model = Map(order);

            var storedCode = HttpContext.Session.GetInt32(cellPhone);
            if (storedCode == null)
                return View("CellPhone", model);

            if (storedCode != code)
            {
                model.Errors["code"] = "Отличается от отправленного";
                return View("Confirmation", model);
            }

            order.CellPhone = cellPhone;
            orderRepository.Update(order);

            HttpContext.Session.Remove(cellPhone);

            model.Methods = deliveryServices.ToDictionary(service => service.Uid, service => service.Title);

            return View("DeliveryMethod", model);
        }

        [HttpPost]
        public IActionResult StartDelivery(int id, Guid uid)
        {
            var deliveryService = deliveryServices.Single(service => service.Uid == uid);

            var order = orderRepository.GetById(id);
            var form = deliveryService.CreateForm(order);

            return View("DeliveryStep", form);
        }

        [HttpPost]
        public IActionResult NextDelivery(int id, Guid uid, int step, Dictionary<string, string> values)
        {
            var deliveryService = deliveryServices.Single(service => service.Uid == uid);

            var form = deliveryService.MoveNext(id, step, values);

            if (form.IsFinal)
            {
                var order = orderRepository.GetById(id);
                order.Delivery = deliveryService.GetDelivery(form);
                orderRepository.Update(order);

                var model = Map(order);
                model.Methods = paymentServices.ToDictionary(service => service.Uid, service => service.Title);
                
                return View("PaymentMethod", model);
            }

            return View("DeliveryStep", form);
        }

        [HttpPost]
        public IActionResult StartPayment(int id, Guid uid)
        {
            var paymentService = paymentServices.Single(service => service.Uid == uid);

            var order = orderRepository.GetById(id);
            var form = paymentService.CreateForm(order);

            var webService = webServices.SingleOrDefault(service => service.Uid == uid);
            if (webService != null)
            {
                var model = new PostRedirectModel
                {
                    Uri = webService.PostUri,
                    Description = "Оплата через Яндекс.Кассу",
                    Parameters = form.Fields.ToDictionary(field => field.Name, field => field.Value),
                };

                return View("PostRedirect", model);
            }

            return View("PaymentStep", form);
        }

        [HttpPost]
        public IActionResult NextPayment(int id, Guid uid, int step, Dictionary<string, string> values)
        {
            var paymentService = paymentServices.Single(service => service.Uid == uid);

            var form = paymentService.MoveNext(id, step, values);

            if (form.IsFinal)
            {
                var order = orderRepository.GetById(id);
                order.Payment = paymentService.GetPayment(form);
                orderRepository.Update(order);

                var model = Map(order);
                return View("Finish", model);
            }

            return View("PaymentStep", form);
        }
    }
}