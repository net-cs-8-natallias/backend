using System.IdentityModel.Tokens.Jwt;
using Basket.Host.Services;
using Basket.Host.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var configuration = GetConfiguration();

var builder = WebApplication.CreateBuilder(args);

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:7001";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAudiences = new[] { "basket", "http://localhost:5055" }
        };
        options.SaveToken = true;
        
    });


builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "eShop - Basket HTTP API",
        Version = "v1",
        Description = "The Basket Service HTTP API"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            },
            new[] { "basket.item", "mvc" }
        }
    });
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("http://localhost:7001/connect/authorize"),
                TokenUrl = new Uri("http://localhost:7001/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "mvc", "Access mvc scope" },
                    { "website", "Access website scope" },
                    { "basket.item", "Access basket.item scope" }
                }
            }
        }
    });

    var authority = configuration["Authorization:Authority"];
    
});

builder.Services.AddAuthorization(options =>
{
    configuration.GetSection("Authorization").Bind(options);

    options.AddPolicy("ApiScope", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(claim =>
                (claim.Type == "scope" && claim.Value == "basket.item") ||
                (claim.Type == "scope" && claim.Value == "mvc"))));
});

builder.Services.AddTransient<IBasketService, BasketService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseSwagger()
    .UseSwaggerUI(setup =>
    {
        setup.SwaggerEndpoint($"{configuration["PathBase"]}/swagger/v1/swagger.json", "Basket.API V1");
        setup.OAuthClientId("basketswaggerui");
        setup.OAuthAppName("Basket Swagger UI");
        setup.OAuth2RedirectUrl("http://localhost:5055/swagger/oauth2-redirect.html");
    });

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
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