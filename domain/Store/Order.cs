using System;
using System.Collections.Generic;
using System.Linq;

namespace Store
{
    public class Order
    {
        public int Id { get; }

        private List<OrderItem> items;
        public IReadOnlyCollection<OrderItem> Items
        {
            get { return items; }
        }

        public OrderState State { get; private set; }

        public string CellPhone { get; set; }

        public int TotalCount
        {
            get { return items.Sum(item => item.Count); }
        }

        public decimal TotalAmount
        {
            get { return items.Sum(item => item.Price * item.Count); }
        }

        public Order(int id, OrderState state, IEnumerable<OrderItem> items)
        {
            Id = id;
            State = state;
            this.items = new List<OrderItem>(items);
        }

        public void AddItem(Book book, int count)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            ValidateState(OrderState.Created);
            
            var item = items.SingleOrDefault(x => x.BookId == book.Id);

            if (item != null)
            {
                items.Add(new OrderItem(book.Id, book.Price, item.Count + count));
                items.Remove(item);
            }
            else
                items.Add(new OrderItem(book.Id, book.Price, count));
        }

        public void StartProcess()
        {
            ValidateState(OrderState.Created);

            State = OrderState.ProcessStarted;
        }

        private void ValidateState(OrderState state)
        {
            if (State != state)
                throw new InvalidOperationException("Invalid state.");
        }
    }
}
