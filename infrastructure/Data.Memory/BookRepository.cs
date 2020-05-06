using Store;
using System;
using System.Linq;

namespace Data.Memory
{
    public class BookRepository : IBookRepository
    {
        private readonly Book[] books = new[]
        {
            new Book(1, "Art Of Programming"),
            new Book(2, "Refactoring"),
            new Book(3, "C Programming Language"),
        };

        public Book[] GetAllByAuthorOrTitle(string title)
        {
            return books.Where(book => book.Title.ToLower().Contains(title.ToLower()))
                        .ToArray();
        }
    }
}
