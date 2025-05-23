using Microsoft.AspNetCore.Mvc;

namespace MicroStack.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
