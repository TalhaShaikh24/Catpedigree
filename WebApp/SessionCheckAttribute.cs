using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userJson = context.HttpContext.Request.Cookies["user"];
        var authorizationCookie = context.HttpContext.Request.Cookies["authorization"];

        if (userJson == null || authorizationCookie == null)
        {
            // Cast to Controller to access TempData
            var controller = context.Controller as Controller;
            if (controller != null)
            {
                controller.TempData["SessionTimeoutMessage"] = "Your session has expired. Please log in again.";
            }

            context.Result = new RedirectToActionResult("Login", "Home", null);
        }

        base.OnActionExecuting(context);
    }
}