using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace WebApp
{
    public class RolePermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _screenName;

        public RolePermissionAttribute(string screenName)
        {
            _screenName = screenName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
    
            string userJson = context.HttpContext.Request.Cookies["user"];

            if (string.IsNullOrEmpty(userJson))
            {
                context.Result = new RedirectToActionResult("login", "Home", null);
                return;
            }

            var data = JsonConvert.DeserializeObject<DataObj>(userJson).dataObj;

            if (data.RoleScreenPermission.Any(x => x.ScreenName.Contains(_screenName)))
            {
              
                return;
            }

            context.Result = new RedirectToActionResult("Index", "Dashboard", null);
        }
    }

}
