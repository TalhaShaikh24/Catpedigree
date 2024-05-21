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
        
        public IActionResult ViewListings()
        {
            return View();
        } 
        public IActionResult SingleListing()
        {
            return View();
        } 
        public IActionResult SingleListing2()
        {
            return View();
        }
        public IActionResult NotAllowed()
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
        public Task<object> GetHomePageListings()
        {
            string content = "";

            return HttpClientUtility.CustomHttp(BaseUrl, "api/Listing/GetHomePageListings", content, HttpContext);
        }

		[HttpPost]
		public Task<object> GetAllListingByFilters([FromBody] ListingFilters obj)
		{
			string content = JsonConvert.SerializeObject(obj);

			return HttpClientUtility.CustomHttp(BaseUrl, "api/Listing/GetAllListingByFilters", content, HttpContext);
		}




		[HttpPost]
        public Task<object> GetAllCatType()
        {
            string content = "";

            return HttpClientUtility.CustomHttp(BaseUrl, "api/Listing/GetAllCatType", content, HttpContext);
        }


        [HttpPost]
        public Task<object> GetAllCatCategory()
        {
            string content = "";

            return HttpClientUtility.CustomHttp(BaseUrl, "api/Listing/GetAllCatCategory", content, HttpContext);
        }

  

    }
}
