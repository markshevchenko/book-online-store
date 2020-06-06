using Microsoft.AspNetCore.Http;
using PhoneNumbers;
using Store.Messages;
using Store.Web.App.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Store.Web.App
{
    public class OrderService
    {
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;
        private readonly INotificationService notificationService;
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();

        protected ISession Session => httpContextAccessor.HttpContext.Session;

        public OrderService(IBookRepository bookRepository,
                            IOrderRepository orderRepository,
                            INotificationService notificationService,
                            IHttpContextAccessor httpContextAccessor)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.notificationService = notificationService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public bool TryGetModel(out OrderModel model)
        {
            if (TryGetOrder(out Order order))
            {
                model = Map(order);
                return true;
            }

            model = null;
            return false;
        }

        protected bool TryGetOrder(out Order order)
        {
            if (Session.TryGetCart(out Cart cart))
            {
                order = orderRepository.GetById(cart.OrderId);
                return true;
            }

            order = null;
            return false;
        }

        private OrderModel Map(Order order)
        {
            var books = GetBooks(order);
            var items = from item in order.Items
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
                Items = items.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice,
            };
        }

        public IEnumerable<Book> GetOrderBooks()
        {
            return GetBooks(GetOrder());
        }

        public Order GetOrder()
        {
            if (TryGetOrder(out Order order))
                return order;

            throw new InvalidOperationException("Empty session.");
        }

        protected IEnumerable<Book> GetBooks(Order order)
        {
            var bookIds = order.Items.Select(item => item.BookId);

            return bookRepository.GetAllByIds(bookIds);
        }

        public OrderModel AddBook(int bookId, int count)
        {
            if (count < 1)
                throw new InvalidOperationException("Too few books to add.");

            if (!TryGetOrder(out Order order))
                order = orderRepository.Create();

            var book = bookRepository.GetById(bookId);
            order.AddOrUpdateItem(book, count);
            UpdateSession(order);

            return Map(order);
        }

        private void UpdateSession(Order order)
        {
            var cart = new Cart(order.Id, order.TotalCount, order.TotalPrice);
            Session.Set(cart);
        }

        public OrderModel UpdateBook(int bookId, int count)
        {
            var order = GetOrder();
            order.GetItem(bookId).Count = count;
            
            orderRepository.Update(order);
            UpdateSession(order);

            return Map(order);
        }

        public OrderModel RemoveBook(int bookId)
        {
            var order = GetOrder();
            order.RemoveItem(bookId);

            orderRepository.Update(order);
            UpdateSession(order);

            return Map(order);
        }

        public OrderModel SendConfirmation(string cellPhone)
        {
            var order = GetOrder();
            var model = Map(order);

            try
            {
                var phoneNumber = phoneNumberUtil.Parse(cellPhone, "ru");
                model.CellPhone = phoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);

                var confirmationCode = 1111; // random.Next(1000, 10000) = 1000, 1001, ..., 9998, 9999
                Session.SetInt32(model.CellPhone, confirmationCode);
                notificationService.SendConfirmationCode(cellPhone, confirmationCode);

            }
            catch (NumberParseException)
            {
                model.Errors["cellPhone"] = "Номер телефона не соответствует формату +79876543210";
            }

            return model;
        }

        public OrderModel ConfirmCellPhone(string cellPhone, int confirmationCode)
        {
            int? storedCode = Session.GetInt32(cellPhone);
            if (storedCode == null)
            {
                return new OrderModel
                {
                    Errors = new Dictionary<string, string>
                    {
                        { "cellPhone",  "Что-то случилось. Попробуйте получить код ещё раз." },
                    }
                };
            }

            if (storedCode != confirmationCode)
            {
                return new OrderModel
                {
                    CellPhone = cellPhone,
                    Errors = new Dictionary<string, string>
                    {
                        { "confirmationCode",  "Неверный код. Проверьте и попробуйте ещё раз." },
                    }
                };
            }

            var order = GetOrder();
            order.CellPhone = cellPhone;
            orderRepository.Update(order);

            Session.Remove(cellPhone);

            return Map(order);
        }

        public OrderModel SetDelivery(OrderDelivery delivery)
        {
            var order = GetOrder();
            order.Delivery = delivery;
            orderRepository.Update(order);

            return Map(order);
        }

        public OrderModel SetPayment(OrderPayment payment)
        {
            var order = GetOrder();
            order.Payment = payment;
            orderRepository.Update(order);

            return Map(order);
        }
    }
}
