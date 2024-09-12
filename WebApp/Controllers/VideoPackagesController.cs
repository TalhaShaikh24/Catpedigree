using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.HttpMethods;

namespace WebApp.Controllers
{
    public class VideoPackagesController : Controller
    {
        private string BaseUrl = "";
        public VideoPackagesController(IConfiguration configuration)
        {
            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";

        }

        public IActionResult VideoPlans()
        {
            return View();
        }

        [HttpPost("VideoPackages/GetAllVideoPackages/{currency}")]
        public Task<object> GetAllVideoPackages(string currency)
        {
            if (currency == null)
            {
                currency = "EUR";
            }

            string content = "";
            return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/VideoPackages/GetAllVideoPackages/" + currency, content, HttpContext);
        }
           
        
        [HttpPost]
        public Task<object> BuyPackage([FromBody] VideoPackage obj)
        {
            string content = JsonConvert.SerializeObject(obj);
            return HttpClientUtility.CustomHttp(BaseUrl, "api/VideoPackages/BuyPackage", content, HttpContext);
        }

        [HttpPost]
        public Task<object> VideoAvailablity()
        {
            string content = "";

            return HttpClientUtility.CustomHttp(BaseUrl, "api/VideoPackages/VideoAvailablity", content, HttpContext);
        }


    }
}
