using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Web;
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
            
                return HttpClientUtility.LogInCustomHttp(BaseUrl, "api/Account/Authenticate", content, HttpContext);
            
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
        
        [HttpPost]
        public Task<object> ForgotPassword([FromBody] ForgotPassword obj)
        {

            string content = JsonConvert.SerializeObject(obj);
            try
            {
                return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Account/ForgotPassword", content, HttpContext);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public Task<object> ResetPassword([FromBody] ForgotPassword obj)
        {

            string content = JsonConvert.SerializeObject(obj);
            try
            {
                return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Account/ResetPassword", content, HttpContext);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public Task<object> LogOut()
        {

            string content = "";

            return HttpClientUtility.LogOutCustomHttp(BaseUrl, "api/Account/Logout", content, HttpContext);

        }
    }
}
