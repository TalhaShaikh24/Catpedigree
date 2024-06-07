using Microsoft.AspNetCore.Mvc;
using WebApp.HttpMethods;

namespace WebApp.Controllers
{
    public class VendorController : Controller
    {

        private string BaseUrl = "";
        public VendorController(IConfiguration configuration)
        {
            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";

        }

        public IActionResult Index()
        {
            return View();
        }



        public IActionResult AllVendors()
        {
            return View();
        }


        public async Task<object> GetVednorDataAndList(int Id)
        {
            string content = "";
            var data = await HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Vendor/GetVednorDataAndList/" + Id, content, HttpContext);

            return data;

        }


        public async Task<object> GetAllVendors()
        {
            string content = "";
            var data = await HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Vendor/GetAllVendors", content, HttpContext);

            return data;

        }





    }
}