using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Web;
using WebApp.HttpMethods;

namespace WebApp.Controllers
{
    public class UsefulLinksController : Controller
    {

        private string BaseUrl = "";
        public UsefulLinksController(IConfiguration configuration)
        {
            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";

        }

      


        [HttpPost]
        public Task<object> GetAllUsefulLinksForGuest()
        {
            string content = "";

            return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/UsefulLinks/GetAllUsefulLinksForGuest", content, HttpContext);
        }

    }
}