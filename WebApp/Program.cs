using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using WebApp;

var builder = WebApplication.CreateBuilder(args);

// Load maintenance mode setting from appsettings.json
var isMaintenanceMode = builder.Configuration.GetValue<bool>("MaintenanceMode");

builder.Services.AddSession(options =>
 {
     options.IdleTimeout = TimeSpan.FromDays(5);
     options.Cookie.HttpOnly = true;
     options.Cookie.IsEssential = true;
 });

builder.Configuration.AddJsonFile("appsettings.json");
// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddScoped<SessionCheckAttribute>();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"

);


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// Use the MaintenanceMiddleware
app.UseMiddleware<MaintenanceMiddleware>(isMaintenanceMode);

app.Run();
