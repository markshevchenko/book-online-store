using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Store.Data.EF
{
    internal class BookRepository : IBookRepository
    {
        private readonly StoreDbContext dbContext;

        public BookRepository(StoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Book[] GetAllByIds(IEnumerable<int> bookIds)
        {
            return dbContext.Books
                            .Where(book => bookIds.Contains(book.Id))
                            .Select(Book.Mapper.ToDomain)
                            .ToArray();
        }

        public Book[] GetAllByIsbn(string isbn)
        {
            if (Book.TryFormatIsbn(isbn, out string formattedIsbn))
            {
                return dbContext.Books
                                .Where(book => book.Isbn == formattedIsbn)
                                .Select(Book.Mapper.ToDomain)
                                .ToArray();
            }

            return new Book[0];
        }

        public Book[] GetAllByTitleOrAuthor(string titleOrAuthor)
        {
            var parameter = new SqlParameter("@titleOrAuthor", titleOrAuthor);
            return dbContext.Books
                            .FromSqlRaw(
                                "SELECT * FROM Books WHERE CONTAINS((Author, Title), @titleOrAuthor)",
                                parameter)
                            .AsEnumerable()
                            .Select(Book.Mapper.ToDomain)
                            .ToArray();
        }

        public Book GetById(int id)
        {
            var dto = dbContext.Books
                               .Single(book => book.Id == id);

            return Book.Mapper.ToDomain(dto);
        }
    }
}
