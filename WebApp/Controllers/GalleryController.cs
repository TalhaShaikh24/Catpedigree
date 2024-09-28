using ClassLibrary;
using ClassLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using WebApp.HttpMethods;

namespace WebApp.Controllers
{
    public class GalleryController : Controller
    {

        private string BaseUrl = "";
        public GalleryController(IConfiguration configuration)
        {
            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";

        }

        [HttpPost]
        public Task<object> GetAllGallery()
        {
          
            return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Gallery/GetAllGallery", "", HttpContext);

        }


    }
}
