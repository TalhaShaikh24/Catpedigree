using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        } 
        public IActionResult AddCategory()
        {
            return View();
        }
    }
}
