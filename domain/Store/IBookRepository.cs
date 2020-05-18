using System.Collections.Generic;

namespace Store
{
    public interface IBookRepository
    {
        Book GetById(int id);
        
        Book[] GetAllByIsbn(string isbn);

        Book[] GetAllByAuthorOrTitle(string authorOrTitle);

        Book[] GetAllByIds(IEnumerable<int> bookIds);
    }
}
