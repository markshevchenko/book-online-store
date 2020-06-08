using Store.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Store
{
    public class OrderItemCollection : IReadOnlyCollection<OrderItem>
    {
        private readonly OrderDto orderDto;
        private readonly List<OrderItem> items;

        public OrderItemCollection(OrderDto orderDto)
        {
            if (orderDto == null)
                throw new ArgumentNullException(nameof(orderDto));

            this.orderDto = orderDto;

            items = orderDto.Items
                            .Select(OrderItem.Mapper.ToDomain)
                            .ToList();
        }

        public int Count => items.Count;

        public OrderItem this[int bookId]
        {
            get
            {
                if (TryGet(bookId, out OrderItem orderItem))
                    return orderItem;

                throw new InvalidOperationException("Book not found.");
            }
        }

        public IEnumerator<OrderItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (items as IEnumerable).GetEnumerator();
        }

        public bool TryGet(int bookId, out OrderItem orderItem)
        {
            var index = items.FindIndex(item => item.BookId == bookId);
            if (index >= 0)
            {
                orderItem = items[index];
                return true;
            }

            orderItem = null;
            return false;
        }

        public OrderItem Add(int bookId, decimal bookPrice, int count)
        {
            if (TryGet(bookId, out OrderItem orderItem))
                throw new InvalidOperationException("Book already exists.");

            var orderItemDto = OrderItem.Factory.CreateDto(orderDto, bookId, bookPrice, count);
            orderDto.Items.Add(orderItemDto);

            orderItem = OrderItem.Mapper.ToDomain(orderItemDto);
            items.Add(orderItem);

            return orderItem;
        }

        public void Remove(int bookId)
        {
            var index = items.FindIndex(item => item.BookId == bookId);
            if (index == -1)
                throw new InvalidOperationException("Can't find book to remove from order.");

            orderDto.Items.RemoveAt(index);
            items.RemoveAt(index);
        }
    }
}
