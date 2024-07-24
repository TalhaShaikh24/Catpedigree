using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.HttpMethods;

namespace WebApp.Controllers
{
    public class PromotionPackageController : Controller
    {
        private string BaseUrl = "";
        public PromotionPackageController(IConfiguration configuration)
        {
            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";

        }

        public IActionResult Index()
        {
            return View();
        }

        public Task<object> GetAllPromotionPackages()
        {
            string content = "";
            return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/PromotionPackage/GetAllPromotionPackages", content, HttpContext);
        }


        [HttpPost]
        public Task<object> BuyPackage([FromBody] PromotionPackages obj)
        {
            string content = JsonConvert.SerializeObject(obj);
            return HttpClientUtility.CustomHttp(BaseUrl, "api/PromotionPackage/BuyPackage", content, HttpContext);
        }



        public async Task<object> GetPromotionCost(int Id)
        {
            string content = "";
            var data = await HttpClientUtility.CustomHttp(BaseUrl, "api/PromotionPackage/GetPromotionCost/" + Id, content, HttpContext);

            return data;

        }

    }
}
