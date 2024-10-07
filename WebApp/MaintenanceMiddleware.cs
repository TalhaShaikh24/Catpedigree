namespace WebApp
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly bool _isMaintenanceMode;

        public MaintenanceMiddleware(RequestDelegate next, bool isMaintenanceMode)
        {
            _next = next;
            _isMaintenanceMode = isMaintenanceMode;
        }

        public async Task Invoke(HttpContext context)
        {
            if (_isMaintenanceMode)
            {
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("<h1>We'll be back soon!</h1><p>The site is currently undergoing maintenance. Please check back later.</p>");
                return;
            }

            await _next(context);
        }
    }

}
