using Moq;
using Xunit;

namespace Store.Tests
{
    public class BookServiceTests
    {
        [Fact]
        public void GetAllBooksByQuery_WithIsbn_CallsGetByIsbn()
        {
            var bookRepositoryStub = new Mock<IBookRepository>();
            bookRepositoryStub.Setup(x => x.GetAllByIsbn(It.IsAny<string>()))
                              .Returns(new[] { new Book(1, "", "", "") });

            bookRepositoryStub.Setup(x => x.GetAllByAuthorOrTitle(It.IsAny<string>()))
                              .Returns(new[] { new Book(1000, "", "", "") });

            var bookService = new BookService(bookRepositoryStub.Object);

            var actual = bookService.GetAllBooksByQuery("ISBN 1234-5678 90");

            Assert.Collection(actual, book => Assert.Equal(1, book.Id));
        }
    }
}
