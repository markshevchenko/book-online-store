using System.Linq;
using Xunit;

namespace Store.Tests
{
    public class OrderTests
    {
        [Fact]
        public void TotalCount_WithEmptyItems_ReturnsZero()
        {
            var order = new Order(1, Enumerable.Empty<OrderItem>());

            Assert.Equal(0, order.TotalCount);
        }

        [Fact]
        public void TotalCount_WithNonEmptyItems_CalculatesTotalCount()
        {

            var order = new Order(1, Enumerable.Empty<OrderItem>());
            order.Items.Add(bookId: 1, price: 10m, count: 3);
            order.Items.Add(bookId: 2, price: 100m, count: 5);

            Assert.Equal(3 + 5, order.TotalCount);
        }

        [Fact]
        public void TotalAmount_WithEmptyItems_ReturnsZero()
        {
            var order = new Order(1, Enumerable.Empty<OrderItem>());

            Assert.Equal(0m, order.TotalAmount);
        }

        [Fact]
        public void TotalAmount_WithNonEmptyItems_CalculatsTotalAmount()
        {
            var order = new Order(1, Enumerable.Empty<OrderItem>());
            order.Items.Add(1, 10m, 3);
            order.Items.Add(2, 100m, 5);

            Assert.Equal(3 * 10m + 5 * 100m, order.TotalAmount);
        }

        const int bookId1 = 2;
        const decimal bookPrice1 = 10m;

        const int bookId2 = 3;
        const decimal bookPrice2 = 20m;

        [Fact]
        public void AddItem_WithNewBookId_AddsItem()
        {
            var order = new Order(1, Enumerable.Empty<OrderItem>());
            order.Items.Add(bookId1, bookPrice1, 3);
            order.Items.Add(bookId2, bookPrice2, 5);

            Assert.Collection(order.Items,
                              item =>
                              {
                                  Assert.Equal(bookId1, item.BookId);
                                  Assert.Equal(3, item.Count);
                              },
                              item =>
                              {
                                  Assert.Equal(bookId2, item.BookId);
                                  Assert.Equal(5, item.Count);
                              });
        }

        [Fact]
        public void AddItem_WithExistingBookId_UpdatesItem()
        {
            var order = new Order(1, Enumerable.Empty<OrderItem>());
            order.Items.Add(bookId1, bookPrice1, 3);
            order.Items.Add(bookId1, bookPrice1, 5);

            Assert.Collection(order.Items,
                              item =>
                              {
                                  Assert.Equal(bookId1, item.BookId);
                                  Assert.Equal(3 + 5, item.Count);
                              });
        }
    }
}
