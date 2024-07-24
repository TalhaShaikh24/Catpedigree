using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.HttpMethods;

namespace WebApp.Controllers
{
    public class CookieConsentController : Controller
    {
        private string BaseUrl = "";
        public CookieConsentController(IConfiguration configuration)
        {
            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";

        }


        [HttpPost]
        public Task<object>  SetConsent(bool consent)
        {
            bool content = true;

            return HttpClientUtility.CustomHttpWithoutTokenBool(BaseUrl, "api/CookieConsent/SetConsent", content, HttpContext);
        }


    }
}
