using Store.Data;
using System;
using Xunit;

namespace Store.Tests
{
    public class OrderItemTests
    {
        [Fact]
        public void CreateDto_WithZeroCount_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = 0;
                OrderItem.Factory.CreateDto(new OrderDto(), 1, 10m, count);
            });
        }

        [Fact]
        public void CreateDto_WithNegativeCount_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = -1;
                OrderItem.Factory.CreateDto(new OrderDto(), 1, 10m, count);
            });
        }

        [Fact]
        public void CreateDto_WithPositiveCount_SetsCount()
        {
            var orderItem = OrderItem.Factory.CreateDto(new OrderDto(), 1, 10m, 30);

            Assert.Equal(1, orderItem.BookId);
            Assert.Equal(10m, orderItem.Price);
            Assert.Equal(30, orderItem.Count);
        }

        [Fact]
        public void Count_WithNegativeValue_ThrowsArgumentOfRangeException()
        {
            var orderItemDto = OrderItem.Factory.CreateDto(new OrderDto(), 1, 10m, 30);
            var orderItem = OrderItem.Mapper.ToDomain(orderItemDto);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                orderItem.Count = -1;
            });
        }

        [Fact]
        public void Count_WithZeroValue_ThrowsArgumentOfRangeException()
        {
            var orderItemDto = OrderItem.Factory.CreateDto(new OrderDto(), 1, 10m, 30);
            var orderItem = OrderItem.Mapper.ToDomain(orderItemDto);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                orderItem.Count = 0;
            });
        }

        [Fact]
        public void Count_WithPositiveValue_SetsValue()
        {
            var orderItemDto = OrderItem.Factory.CreateDto(new OrderDto(), 1, 10m, 30);
            var orderItem = OrderItem.Mapper.ToDomain(orderItemDto);

            orderItem.Count = 10;

            Assert.Equal(10, orderItem.Count);
        }
    }
}
