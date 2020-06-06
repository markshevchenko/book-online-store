using Microsoft.AspNetCore.Mvc;
using Store.YandexKassa.Areas.YandexKassa.Models;
using System.Collections.Generic;

namespace Store.YandexKassa.Areas.YandexKassa.Controllers
{
    [Area("YandexKassa")]
    public class HomeController : Controller
    {
        public IActionResult Index(Dictionary<string, string> parameters, string returnUri)
        {
            var model = new ParametersModel
            {
                Parameters = parameters,
                ReturnUri = returnUri,
            };

            return View(model);
        }

        public IActionResult Callback(Dictionary<string, string> parameters, string returnUri)
        {
            var model = new ParametersModel
            {
                Parameters = parameters,
                ReturnUri = returnUri,
            };

            return View(model);
        }
    }
}
