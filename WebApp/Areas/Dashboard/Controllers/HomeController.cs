using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.HttpMethods;

namespace WebApp.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class HomeController : Controller
    {

        private string BaseUrl = "";
        public HomeController(IConfiguration configuration)
        {
            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";

        }
        public IActionResult Index()
        {
            return View();
        } 
        public IActionResult AddCategory()
        {
            return View();
        }

        [Route("Dashboard/Profile")]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpPost]
        [Route("Dashboard/GetAllDashboard")]
        public Task<object> GetAllDashboard()
        {

            return HttpClientUtility.CustomHttp(BaseUrl, "api/Dashboard/GetAllDashboard", "", HttpContext);

        }
    }
}
