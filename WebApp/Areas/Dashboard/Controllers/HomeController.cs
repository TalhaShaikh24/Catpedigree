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

        [Route("Dashboard/MyListing")]
        public IActionResult MyListing()
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

        [HttpPost]
        [Route("Dashboard/UpdateProfile")]
        public Task<object> UpdateProfile()
        {

            return HttpClientUtility.CustomHttp(BaseUrl, "api/Dashboard/UpdateProfile", "", HttpContext);

        }
    }
}
