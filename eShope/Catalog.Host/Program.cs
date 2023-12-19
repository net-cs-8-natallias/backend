
using Catalog.Host.Configurations;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories;
using Catalog.Host.Services;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;


var configuration = GetConfiguration();
var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<CatalogConfigurations>(configuration);
builder.Services.AddDbContextFactory<ApplicationDbContext>(opts => opts.UseNpgsql(configuration["ConnectionString"]));
builder.Services.AddTransient<ICatalogRepository<CatalogItem>, ItemsCatalogRepository>();
builder.Services.AddTransient<ICatalogRepository<CatalogType>, TypesCatalogRepository>();
builder.Services.AddTransient<ICatalogRepository<CatalogBrand>, BrandsCatalogRepository>();
builder.Services.AddTransient<IItemsCatalogRepository, ItemsCatalogRepository>();
builder.Services.AddTransient<ICatalogService<CatalogItem>, CatalogItemService>();
builder.Services.AddTransient<ICatalogService<CatalogType>, CatalogTypeService>();
builder.Services.AddTransient<ICatalogService<CatalogBrand>, CatalogBrandService>();
builder.Services.AddTransient<IBffService, BffService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

CreateDbIfNotExists(app);
app.Run();

Log.CloseAndFlush();

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}

void CreateDbIfNotExists(IHost host)
{
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider; 
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        DbInitializer.Initialize(context).GetAwaiter().GetResult();
    }
    catch (Exception e)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(e, "An error occured creating DB");
    }
    
    
}