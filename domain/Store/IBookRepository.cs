namespace Store
{
    public interface IBookRepository
    {
        Book[] GetAllByIsbn(string isbn);

        Book[] GetAllByAuthorOrTitle(string authorOrTitle);
    }
}
