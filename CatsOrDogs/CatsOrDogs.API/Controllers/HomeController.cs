using Microsoft.AspNetCore.Mvc;

namespace CatsOrDogs.API.Controllers
{
    /// <summary>
    /// Контроллер по умолчанию
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary/>
        public IActionResult Index() => new RedirectResult("~/help");
    }
}