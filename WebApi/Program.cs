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
        options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                builder.WithOrigins("https://localhost:7297")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            });
    });


builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IDapper, Dapperr>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IListingRepository, ListingRepository>();
builder.Services.AddTransient<IPackagesRepository, PackagesRepository> ();
builder.Services.AddTransient<IDashboardRepository, DashboardRepository> ();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();

app.Run();
