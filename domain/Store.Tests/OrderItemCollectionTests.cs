using Store.Data;
using System;
using System.Collections.Generic;
using Xunit;

namespace Store.Tests
{
    public class OrderItemCollectionTests
    {
        [Fact]
        public void IndexerGet_WithExistingItem_ReturnsItem()
        {
            var order = CreateTestOrder();

            var orderItem = order.Items[1];

            Assert.Equal(3, orderItem.Count);
        }

        private static Order CreateTestOrder()
        {
            return new Order(new OrderDto
            {
                Id = 1,
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto { BookId = 1, Price = 10m, Count = 3},
                    new OrderItemDto { BookId = 2, Price = 100m, Count = 5},
                }
            });
        }

        [Fact]
        public void IndexerGet_WithNonExistingItem_ThrowsInvalidOperationException()
        {
            var order = CreateTestOrder();

            Assert.Throws<InvalidOperationException>(() =>
            {
                var item = order.Items[100];
            });
        }

        [Fact]
        public void Add_WithExistingItem_ThrowsInvalidOperationException()
        {
            var order = CreateTestOrder();

            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Add(1, 10m, 3);
            });
        }

        [Fact]
        public void Add_WithNewItem_AddsCount()
        {
            var order = CreateTestOrder();

            order.Items.Add(4, 30m, 10);

            Assert.Equal(10, order.Items[4].Count);
        }

        [Fact]
        public void Remove_WithExistingItem_RemovesItem()
        {
            var order = CreateTestOrder();

            order.Items.Remove(1);

            Assert.Collection(order.Items,
                              item => Assert.Equal(2, item.BookId));
        }

        [Fact]
        public void Remove_WithNonExistingItem_ThrowsInvalidOperationException()
        {
            var order = CreateTestOrder();

            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Remove(100);
            });
        }
    }
}
