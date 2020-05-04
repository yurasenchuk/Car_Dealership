using Microsoft.AspNetCore.Mvc;

namespace Car_Dealership.Controllers
{
    public class Home : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}