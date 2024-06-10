using ClassLibrary;
using ClassLibrary.ViewModels;
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

        public IActionResult Index(string tokens)
        {
            var token = HttpContext.Request.Cookies["authorization"];

            if (!string.IsNullOrEmpty(token))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }


        public IActionResult ViewListings()
        {
            return View();
        }





      

        public async Task<IActionResult> SingleListing(int listingId)
        {
            
            string content = "";

            var json = await GetListingDetailById(listingId);

          

            ListingForView listing = JsonConvert.DeserializeObject<ListingForView>(json.ToString());

            var model = JsonConvert.DeserializeObject<Listing>(json.ToString());


            if (listing.data.Listing != null&& listing.data.Listing.CategoryName!=null)
            {


                if (listing.data.Listing.CategoryName.ToLower() == "Pedigree".ToLower())
                {
                    var Token = HttpContext.Request.Cookies["authorization"];

                    if (Token == null)
                    {


                        return RedirectToAction("Login", "Home");

                    }
                    else
                    {
                        return View(listing);

                    }

                }
                else
                {
                    return View(listing);

                }

            }

            else
            {
                return View(listing);


            }
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

            return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Listing/GetHomePageListings", content, HttpContext);
        }



        [HttpPost]
		public Task<object> GetAllListingByFilters([FromBody] Listing obj)
		{
			string content = JsonConvert.SerializeObject(obj);

			return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Listing/GetAllListingByFilters", content, HttpContext);
		}

        [HttpPost]
		public Task<object> GetSingleListing([FromBody] Listing obj)
		{
			string content = JsonConvert.SerializeObject(obj);

			return HttpClientUtility.CustomHttp(BaseUrl, "api/Listing/GetSingleListing", content, HttpContext);
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
        
        [HttpPost]
        public Task<object> GetAllPackage()
        {
            string content = "";

            return HttpClientUtility.CustomHttp(BaseUrl, "api/Listing/GetAllPackage", content, HttpContext);
        }


        [HttpPost]
        public Task<object> GetAllDropdowns()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Listing/GetAllDropdowns", "", HttpContext);

        }



        [HttpPost]
        public async Task<object> GetListingDetailById(int Id)
        {

            var data = await HttpClientUtility.CustomHttp(BaseUrl, "api/Listing/VideoAvailablity/" + Id, "", HttpContext);

            return data;

        }


        [HttpPost]
        public async Task<object> SelectPackageListingShowValidation(int Id)
        {

            var data = await HttpClientUtility.CustomHttp(BaseUrl, "api/Listing/SelectPackageListingShowValidation/" + Id, "", HttpContext);

            return data;

        }



    }
}
