using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.HttpMethods;

namespace WebApp.Controllers
{
    public class ShowController : Controller
    {
        private string BaseUrl = "";
        public ShowController(IConfiguration configuration)
        {
            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";

        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult ShowDetails()
        {
            // This page will exclude the Style.css CDN link
            ViewData["ExcludeCDN"] = true;
            return View();
        }

        [HttpPost]
        public Task<object> GetAllShowDetails(int Id)
        {
            string content = "";

            return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Show/GetAllShowDetails/" + Id, content, HttpContext);
        }

        [HttpPost]
        public Task<object> GetAllShowsPagination([FromBody] Show obj)
        {
            string content = JsonConvert.SerializeObject(obj);

            return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Show/GetAllShowsPagination", content, HttpContext);
        }
    }
}
