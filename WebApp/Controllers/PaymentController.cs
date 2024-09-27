using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.HttpMethods;

namespace WebApp.Controllers
{
    public class PaymentController : Controller
    {
        private string BaseUrl = "";

        public PaymentController(IConfiguration configuration)
        {
            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";

        }
        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public Task<object> createcheckoutsession([FromBody] CheckoutSessionRequest obj)
        {
            string content = JsonConvert.SerializeObject(obj);
            return HttpClientUtility.CustomHttp(BaseUrl, "api/Payment/create-checkout-session", content, HttpContext);
        }

    }
}
