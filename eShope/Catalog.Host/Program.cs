using System.IdentityModel.Tokens.Jwt;
using Catalog.Host.Configurations;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories;
using Catalog.Host.Services;
using Catalog.Host.Services.Interfaces;
using Infrastructure;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var configuration = GetConfiguration();
var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:7001";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });



 builder.Services.AddSwaggerGen(options =>
 {
     options.SwaggerDoc("v1", new OpenApiInfo
     {
         Title = "eShop- Catalog HTTP API",
         Version = "v1",
         Description = "The Catalog Service HTTP API"
     });
     options.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
         {
             new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
             },
             new[] { "basket.item", "mvc", "order.orderitem", "ApiScope" }
         }
     });
      options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
      {
          Type = SecuritySchemeType.OAuth2,
          Flows = new OpenApiOAuthFlows()
          {
              Implicit = new OpenApiOAuthFlow()
              {
                  AuthorizationUrl = new Uri("http://localhost:7001/connect/authorize"),
                  TokenUrl = new Uri("http://localhost:7001/connect/token"),
                  Scopes = new Dictionary<string, string>()
                  {
                      { "mvc", "web" },
                      { "catalog.catalogitem", "catalog" },
                      { "order.orderitem", "order" }
                  }
              }
          }
      }
     );

     options.OperationFilter<AuthorizeCheckOperationFilter>();
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "catalogitem");
    });
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
})
.AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        builder => builder
            .SetIsOriginAllowed(host => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.Configure<CatalogConfigurations>(configuration);
builder.Services.AddDbContextFactory<ApplicationDbContext>(opts => opts.UseNpgsql(configuration["ConnectionString"]));
builder.Services.AddScoped<IDbContextWrapper<ApplicationDbContext>, DbContextWrapper<ApplicationDbContext>>();


builder.Services.AddTransient<ICatalogRepository<CatalogItem>, ItemsCatalogRepository>();
builder.Services.AddTransient<ICatalogRepository<CatalogType>, TypesCatalogRepository>();
builder.Services.AddTransient<ICatalogRepository<CatalogBrand>, BrandsCatalogRepository>();
builder.Services.AddTransient<IItemsCatalogRepository, ItemsCatalogRepository>();
builder.Services.AddTransient<ICatalogService<CatalogItem>, CatalogItemService>();
builder.Services.AddTransient<ICatalogService<CatalogType>, CatalogTypeService>();
builder.Services.AddTransient<ICatalogService<CatalogBrand>, CatalogBrandService>();
builder.Services.AddTransient<IBffService, BffService>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CorsPolicy");

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute().RequireAuthorization("ApiScope");
    endpoints.MapControllers();//.RequireAuthorization -> for all controllers
});

//app.MapControllers();

CreateDbIfNotExists(app);
app.Run();

Log.CloseAndFlush();

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false, true)
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