using Microsoft.AspNetCore.Mvc;

namespace NetCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
