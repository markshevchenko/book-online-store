using System;

namespace Store
{
    public class OrderItem
    {
        public int BookId { get; }

        public decimal Price { get; }

        public int Count { get; }

        public OrderItem(int bookId, decimal price, int count)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than zero.");

            BookId = bookId;
            Price = price;
            Count = count;
        }
    }
}
