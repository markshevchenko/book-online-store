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
            var order = new Order(1, new[]
                                     {
                                         new OrderItem(1, 10m, 3),
                                         new OrderItem(2, 100m, 5),
                                     });

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
            var order = new Order(1, new[]
                                     {
                                         new OrderItem(1, 10m, 3),
                                         new OrderItem(2, 100m, 5),
                                     });

            Assert.Equal(3 * 10m + 5 * 100m, order.TotalAmount);
        }

        const int bookId1 = 2;
        const decimal bookPrice1 = 10m;

        const int bookId2 = 3;
        const decimal bookPrice2 = 20m;

        [Fact]
        public void AddItem_WithNewBookId_AddsItem()
        {
            var existingItem = new OrderItem(bookId1, bookPrice1, 3);
            var order = new Order(1, new[] { existingItem });
            var newBook = new Book(bookId2, "", "", "", "", bookPrice2);

            order.AddItem(newBook, 5);

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
            var existingItem = new OrderItem(bookId1, bookPrice1, 3);
            var order = new Order(1, new[] { existingItem });
            var existingBook = new Book(bookId1, "", "", "", "", bookPrice1);

            order.AddItem(existingBook, 5);

            Assert.Collection(order.Items,
                              item =>
                              {
                                  Assert.Equal(bookId1, item.BookId);
                                  Assert.Equal(3 + 5, item.Count);
                              });
        }

    }
}
