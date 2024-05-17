using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Web;
using WebApp.HttpMethods;

namespace WebApp.Controllers
{
    public class ListingController : Controller
    {

        private string BaseUrl = "";
        public ListingController(IConfiguration configuration)
        {
            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public Task<object> AddListting([FromForm] Listing obj)
        {

            obj.Id = 0;

            return HttpClientUtility.CustomHttpListing(BaseUrl, "api/Listing/AddListing", obj, HttpContext);

        }
              
        
        
        [HttpPost]
        public Task<object> UpdateListing([FromForm] Listing obj)
        {


            return HttpClientUtility.CustomHttpListing(BaseUrl, "api/Listing/UpdateListing", obj, HttpContext);

        }


        [HttpPost]
        public Task<object> GetAllPackage()
        {
            string content = "";

            return HttpClientUtility.CustomHttp("https://localhost:7280/", "api/Listing/GetAllPackage", content, HttpContext);
        }



        [HttpPost]
        public Task<object> GetAllCatType()
        {
            string content = "";

            return HttpClientUtility.CustomHttp("https://localhost:7280/", "api/Listing/GetAllCatType", content, HttpContext);
        }


        [HttpPost]
        public Task<object> GetAllCatCategory()
        {
            string content = "";

            return HttpClientUtility.CustomHttp("https://localhost:7280/", "api/Listing/GetAllCatCategory", content, HttpContext);
        }

  

    }
}
