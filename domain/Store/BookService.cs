namespace Store
{
    public class BookService
    {
        private readonly IBookRepository bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public Book[] GetAllBooksByQuery(string query)
        {
            if (Book.IsIsbnValid(query))
            {
                var normalizedIsbn = Book.NormalizeIsbn(query);

                return bookRepository.GetAllByIsbn(normalizedIsbn);
            }

            return bookRepository.GetAllByAuthorOrTitle(query);
        }
    }
}
