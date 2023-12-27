using MVC;
using MVC.Services;
using MVC.Services.Interfaces;
using Serilog;

var configuration = GetConfiguration();

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<AppSettings>(configuration);

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddHttpClient();
builder.Services.AddTransient<IHttpClientService, HttpClientService>();
builder.Services.AddTransient<ICatalogService, CatalogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default", "{controller=Catalog}/{action=Index}/{id?}");
    endpoints.MapControllerRoute("defaultError", "{controller=Error}/{action=Error}");
    endpoints.MapControllers();
});

app.Run();

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}
