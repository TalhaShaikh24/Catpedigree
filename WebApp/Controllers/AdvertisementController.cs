using Microsoft.AspNetCore.Mvc;
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

        public async Task<object> GetHomeAdvertisments(int Id)
        {
            string content = "";
            var data = await HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Advertisement/GetHomeAdvertisments/" + Id, content, HttpContext);

            return data;

        }

		
		public Task<object> GetAdvertisementPackage()
		{
			var content = "";

			return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Advertisement/GetAdvertisementPackage", content, HttpContext);

		}
	}
}
