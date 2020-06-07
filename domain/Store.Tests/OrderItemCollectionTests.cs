using System;
using Xunit;

namespace Store.Tests
{
    public class OrderItemCollectionTests
    {
        [Fact]
        public void IndexerGet_WithExistingItem_ReturnsItem()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1, 10m, 3),
                new OrderItem(2, 100m, 5),
            });

            var orderItem = order.Items[1];

            Assert.Equal(3, orderItem.Count);
        }

        [Fact]
        public void IndexerGet_WithNonExistingItem_ThrowsInvalidOperationException()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1, 10m, 3),
                new OrderItem(2, 100m, 5),
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                var item = order.Items[100];
            });
        }

        [Fact]
        public void Add_WithExistingItem_ThrowsInvalidOperationException()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1, 10m, 3),
                new OrderItem(2, 100m, 5),
            });

            var book = new Book(1, null, null, null, null, 0m);

            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Add(book.Id, book.Price, 3);
            });
        }

        [Fact]
        public void Add_WithNewItem_AddsCount()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1, 10m, 3),
                new OrderItem(2, 100m, 5),
            });

            var book = new Book(4, null, null, null, null, 0m);

            order.Items.Add(book.Id, book.Price, 10);

            Assert.Equal(10, order.Items[4].Count);
        }

        [Fact]
        public void Remove_WithExistingItem_RemovesItem()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1, 10m, 3),
                new OrderItem(2, 100m, 5),
            });

            order.Items.Remove(1);

            Assert.Collection(order.Items,
                              item => Assert.Equal(2, item.BookId));
        }

        [Fact]
        public void Remove_WithNonExistingItem_ThrowsInvalidOperationException()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1, 10m, 3),
                new OrderItem(2, 100m, 5),
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Remove(100);
            });
        }
    }
}
