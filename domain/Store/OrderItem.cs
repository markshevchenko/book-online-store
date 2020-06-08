using Store.Data;
using System;

namespace Store
{
    public class OrderItem
    {
        private readonly OrderItemDto dto;

        public int BookId => dto.BookId;

        public int Count
        {
            get { return dto.Count; }
            set
            {
                ThrowIfInvalidCount(value);

                dto.Count = value;
            }
        }

        public decimal Price
        {
            get => dto.Price;
            set => dto.Price = value;
        }

        internal OrderItem(OrderItemDto dto)
        {
            this.dto = dto;
        }

        private static void ThrowIfInvalidCount(int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("Count must be greater than zero.");
        }

        public static class Factory
        {
            public static OrderItemDto CreateDto(OrderDto order, int bookId, decimal price, int count)
            {
                if (order == null)
                    throw new ArgumentNullException(nameof(order));
                
                ThrowIfInvalidCount(count);

                return new OrderItemDto
                {
                    BookId = bookId,
                    Price = price,
                    Count = count,
                    Order = order,
                };
            }
        }

        public static class Mapper
        {
            public static OrderItem ToDomain(OrderItemDto dto) => new OrderItem(dto);

            public static OrderItemDto ToDto(OrderItem domain) => domain.dto;
        }
    }
}
