using Microsoft.AspNetCore.Mvc;

namespace NetCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //url param di default a casua di program.cs
        public IActionResult Details(int id)
        {
            //ViewData["pizzaId"] = id;
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
