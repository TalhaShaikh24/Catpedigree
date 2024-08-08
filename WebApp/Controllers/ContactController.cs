using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.HttpMethods;

namespace WebApp.Controllers
{
    public class ContactController : Controller
    {
        private string BaseUrl = "";
        public ContactController(IConfiguration configuration)
        {
            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";

        }


        [HttpPost]
        public Task<object>  AddContact([FromBody] Contact contactInfo)
        {
            string content = JsonConvert.SerializeObject(contactInfo);

            return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Contact/AddContact", content, HttpContext);
        }


    }
}
