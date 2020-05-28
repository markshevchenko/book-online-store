using System;

namespace Store
{
    public class OrderItem
    {
        private readonly Order order;

        public int BookId { get; }

        public decimal Price { get; }

        private int count;
        public int Count
        {
            get { return count; }
            set
            {
                if (order.State != OrderState.Pushing)
                    throw new InvalidOperationException("Invalid order state.");

                ThrowIfInvalidCount(value);

                count = value;
            }
        }

        public OrderItem(Order order, int bookId, decimal price, int count)
        {
            ThrowIfInvalidCount(count);

            this.order = order;

            BookId = bookId;
            Price = price;
            Count = count;
        }

        private static void ThrowIfInvalidCount(int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("Count must be greater than zero.");
        }
    }
}
