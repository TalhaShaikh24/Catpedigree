using ClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using WebApi.IRepositories;
using WebApi.Utility;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CookieConsentController : ControllerBase
    {
       
      
        public CookieConsentController()
        {
            
          
        }

        [HttpPost("SetConsent")]
        public IActionResult SetConsent(bool consent)
        {
            if (consent)
            {
                try
                {
                    Response.Cookies.Append("CookieConsent", "true", new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1),
                        HttpOnly = true,
                        Secure = true
                    });
                }
                catch (Exception)
                {

                    throw;
                }
               
            }

            return Ok();
        }


    }
}
