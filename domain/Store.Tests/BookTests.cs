using System;
using Xunit;

namespace Store.Tests
{
    public class BookTests
    {
        [Fact]
        public void IsIsbnValid_WithNull_ReturnsFalse()
        {
            var isIsnbnValid = Book.IsIsbnValid(null);

            Assert.False(isIsnbnValid);
        }

        [Fact]
        public void IsIsbnValid_WithSpaces_ReturnsFalse()
        {
            var isIsnbnValid = Book.IsIsbnValid("    ");

            Assert.False(isIsnbnValid);
        }

        [Fact]
        public void IsIsbnValid_WithInvalidIsbn_ReturnsFalse()
        {
            var isIsnbnValid = Book.IsIsbnValid("ISBN 1234");

            Assert.False(isIsnbnValid);
        }

        [Fact]
        public void IsIsbnValid_WithIsbn10_ReturnsTrue()
        {
            var isIsnbnValid = Book.IsIsbnValid("ISBN 12345-67980");

            Assert.True(isIsnbnValid);
        }

        [Fact]
        public void IsIsbnValid_WithIsbn13_ReturnsTrue()
        {
            var isIsnbnValid = Book.IsIsbnValid("ISBN 12345-67980-123");

            Assert.True(isIsnbnValid);
        }

        [Fact]
        public void NormalizeIsbn_WithNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Book.NormalizeIsbn(null));
        }

        [Fact]
        public void NormalizeIsbn_WithDashesAndSpaces_RemovesDashesAndSpaces()
        {
            var actual = Book.NormalizeIsbn("ISBN-123-456 78 90");

            Assert.Equal("ISBN1234567890", actual);
        }
    }
}
