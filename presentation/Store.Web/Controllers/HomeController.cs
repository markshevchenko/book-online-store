using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Store.Web.Models;

namespace Store.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionHandler = HttpContext.Features
                                              .Get<IExceptionHandlerFeature>();

            if (exceptionHandler == null)
                return BadRequest();

            var exception = exceptionHandler.Error;

            if (exception.TargetSite.Name == "ThrowNoElementsException")
                return NotFound();

            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(model);
        }
    }
}
