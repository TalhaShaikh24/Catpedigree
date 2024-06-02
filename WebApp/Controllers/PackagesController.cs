using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.HttpMethods;

namespace WebApp.Controllers
{
    public class PackagesController : Controller
    {
        private string BaseUrl = "";
        public PackagesController(IConfiguration configuration)
        {
            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";

        }

        public IActionResult Pricing()
        {
            return View();
        }

        [HttpPost]
        public Task<object> GetAllPackages()
        {
            string content = "";
            return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Packages/GetAllPackages", content, HttpContext);
        }

        [HttpPost]
        public Task<object> BuyPackage([FromBody] UserPackages obj)
        {
            string content = JsonConvert.SerializeObject(obj);
            return HttpClientUtility.CustomHttp(BaseUrl, "api/Packages/BuyPackage", content, HttpContext);
        }
    }
}
