using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.HttpMethods;

namespace WebApp.Controllers
{

    public class AccountController : Controller
    {
        private string BaseUrl = "";
        public AccountController(IConfiguration configuration)
        {
            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";

        }

        [HttpPost]
        public Task<object> Authenticate([FromBody] Register obj)
        {
            
                string content = JsonConvert.SerializeObject(obj);
            
                return HttpClientUtility.CustomHttp(BaseUrl, "api/Account/Authenticate", content, HttpContext);
            
        }

        [HttpPost]
        public Task<object> RegisterUser([FromForm] Register obj)
        {

            string content = JsonConvert.SerializeObject(obj);
            try
            {
                return HttpClientUtility.CustomHttpIfile(BaseUrl, "api/Account/RegisterUser", obj, HttpContext);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
