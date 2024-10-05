using ClassLibrary;
using Google.Api;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using System.Configuration;
using WebApi.DBManager;
using WebApi.IRepositories;
using WebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowMultipleOrigins",
            builder =>
            {
                builder.WithOrigins("https://catpedigreeworld.com", "http://catpedigreeworld.com", "https://localhost:7297", "http://localhost:7297")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
    }); 

builder.Services.AddSwaggerGen();

// Register HttpClient
builder.Services.AddHttpClient();

builder.Services.AddTransient<IDapper, Dapperr>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IListingRepository, ListingRepository>();
builder.Services.AddTransient<IGalleryRepository, GalleryRepository>();
builder.Services.AddTransient<IPackagesRepository, PackagesRepository> ();
builder.Services.AddTransient<IDashboardRepository, DashboardRepository> ();
builder.Services.AddTransient<IVideoPackagesRepository, VideoPackagesRepository> ();
builder.Services.AddTransient<IBlogRepository, BlogRepository> ();
builder.Services.AddTransient<IPromotionPackageRepository, PromotionPackageRepository> ();
builder.Services.AddTransient<IVendorRepository, VendorRepository> ();
builder.Services.AddTransient<IAdvertisementServices  ,   AdvertisementServices> ();
builder.Services.AddTransient<IContactRepository,   ContactRepository> ();
builder.Services.AddTransient<IMolliePaymentService  , MolliePaymentService> ();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddTransient<IStripeServices, StripeServices>();
builder.Services.AddTransient<IEmailRepository, EmailRepository>();

builder.Services.Configure<ExchangeRateApiSettings>(builder.Configuration.GetSection("ExchangeRateApi"));
builder.Services.AddTransient<ICurrencyConverterService, CurrencyConverterService>();


var app = builder.Build();



// Configure Stripe API key
var stripeSettings = app.Configuration.GetSection("Stripe").Get<StripeSettings>();
StripeConfiguration.ApiKey = stripeSettings.SecretKey;


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowMultipleOrigins");
//app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();

app.Run();
