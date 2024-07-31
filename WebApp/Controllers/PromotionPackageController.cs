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


        [HttpPost("PromotionPackage/GetAllPromotionPackages/{currency}")]
        public Task<object> GetAllPromotionPackages(string currency)
        {

            if (currency == null)
            {
                currency = "EUR";
            }

            string content = "";
            return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/PromotionPackage/GetAllPromotionPackages/"+ currency, content, HttpContext);
        }


        [HttpPost]
        public Task<object> BuyPackage([FromBody] PromotionPackages obj)
        {
            string content = JsonConvert.SerializeObject(obj);
            return HttpClientUtility.CustomHttp(BaseUrl, "api/PromotionPackage/BuyPackage", content, HttpContext);
        }



        [HttpPost]
        public async Task<object> GetPromotionCost([FromBody] PromotionsCostCur obj)
        {


            string content = JsonConvert.SerializeObject(obj); ;

           

            var data = await HttpClientUtility.CustomHttp(BaseUrl, "api/PromotionPackage/GetPromotionCost", content, HttpContext);

            return data;

        }

    }
}
