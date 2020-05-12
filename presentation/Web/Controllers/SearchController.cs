using Microsoft.AspNetCore.Mvc;
using Store;

namespace Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly BookService bookService;

        public SearchController(BookService bookService)
        {
            this.bookService = bookService;
        }

        public IActionResult Index(string query)
        {
            var books = bookService.GetAllBooksByQuery(query);

            return View(books);
        }
    }
}