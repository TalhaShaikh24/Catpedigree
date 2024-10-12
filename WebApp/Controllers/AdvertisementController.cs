using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebApp.HttpMethods;

namespace WebApp.Controllers
{
    public class AdvertisementController : Controller
    {
        private string BaseUrl = "";

        public IActionResult AdvertisementPackages()
        {
            return View();
        }


        public AdvertisementController(IConfiguration configuration)
        {
            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";

        }

        public async Task<object> GetFooterAdvertisments()
        {
            string content = "";
            var data = await HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Advertisement/GetFooterAdvertisments", content, HttpContext);

            return data;

        }
            public async Task<object> GetHomeAdvertisments()
        {
            string content = "";
            var data = await HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Advertisement/GetHomeAdvertisments", content, HttpContext);

            return data;

        }
        
        public async Task<object> GetSidebarAdvertisments(int Id)
        {
            string content = "";
            var data = await HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Advertisement/GetSidebarAdvertisments/"+Id, content, HttpContext);
            return data;

        }

        public async Task<object> GetAllAdsForViewListings(int Id)
        {
            string content = "";
            var data = await HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Advertisement/GetAllAdsForViewListings/", content, HttpContext);
            return data;
        }


        [HttpPost]
        public  object CheckCookiesData()
        {
            bool cookieExists = Request.Cookies.ContainsKey("user");

            if (cookieExists)
            {
                var cookieValue = Request.Cookies["user"];



                var dataObj = JObject.Parse(cookieValue)["dataObj"];
                var roleIds = dataObj["roleIds"]?.ToString();

                if (string.IsNullOrEmpty(roleIds))
                {
                    return false;
                }

                var rolesArray = roleIds.Split(',');
                return rolesArray.Select(role => role.Trim()).Contains("Business Advertiser");

           
            }
            else
            {
                return null;
            }
        }

        public Task<object> GetAdvertisementPackage(string currency)
        {
            if (currency == null)
            {
                currency = "EUR";
            }


            string content = "";
            return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Advertisement/GetAdvertisementPackage/" + currency, content, HttpContext);
        }

    }
}
