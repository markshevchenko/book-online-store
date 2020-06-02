using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Store
{
    public class OrderItemCollection : IReadOnlyCollection<OrderItem>
    {
        private readonly Order order;
        private readonly List<OrderItem> items;

        public OrderItemCollection(Order order, IEnumerable<OrderItem> items)
        {
            this.order = order;
            this.items = new List<OrderItem>(items);
        }

        public int Count => items.Count;

        public OrderItem this[int bookId]
        {
            get
            {
                int index = IndexByBookId(bookId);
                
                return items[index];
            }
        }

        public OrderItem Add(int bookId, decimal price, int count)
        {
            if (order.State != OrderState.Created)
                throw new InvalidOperationException("Invalid order state.");

            var orderItem = items.SingleOrDefault(item => item.BookId == bookId);
            if (orderItem == null)
            {
                orderItem = new OrderItem(order, bookId, price, count);
                items.Add(orderItem);
            }
            else
                orderItem.Count += count;

            return orderItem;
        }

        public void Remove(int bookId)
        {
            if (order.State != OrderState.Created)
                throw new InvalidOperationException("Invalid order state.");

            int index = IndexByBookId(bookId);

            items.RemoveAt(index);
        }

        public IEnumerator<OrderItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (items as IEnumerable).GetEnumerator();
        }

        private int IndexByBookId(int bookId)
        {
            int index = items.FindIndex(item => item.BookId == bookId);

            if (index == -1)
                throw new InvalidOperationException("Specified book not found.");

            return index;
        }
    }
}
