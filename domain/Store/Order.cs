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

        public int TotalCount
        {
            get { return items.Sum(item => item.Count); }
        }

        public decimal TotalAmount
        {
            get { return items.Sum(item => item.Price * item.Count); }
        }

        public Order(int id, IEnumerable<OrderItem> items)
        {
            Id = id;
            this.items = new List<OrderItem>(items);
        }

        public void AddItem(Book book, int count)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));
            
            var item = items.SingleOrDefault(x => x.BookId == book.Id);

            if (item != null)
            {
                items.Add(new OrderItem(book.Id, book.Price, item.Count + count));
                items.Remove(item);
            }
            else
                items.Add(new OrderItem(book.Id, book.Price, count));
        }
    }
}
