using System.Text.Json;
using Catalog.Host;
using StringReader = Catalog.Host.StringReader;

// create server
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// adding controller
builder.Services.AddControllers();

// app will create each time new instance where are injections 
//builder.Services.AddScoped<IStringReader, StringReader>();
// singleton -> app will reuse one instance of class
builder.Services.AddSingleton<IStringReader, StringReader>();

// create app container
var app = builder.Build();

// use -> middleware/pipeline -> ordered
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// redirection from http to https
// app.UseHttpsRedirection();

// app.UseAuthorization();
// app.UseAuthentication();

// mapping controller
app.MapControllers();

// 1 way to create middleware
// not calling another middlewares from pipeline
// http://localhost:5054/WeatherForecast
// http://localhost:5054/WeatherForecast/test
// app.Run(async context =>
// {
//     await context.Response.WriteAsync("Hello world");
// });

// 2 way to create middleware ( with map/delegate )
// calling another middlewares from pipeline
// http://localhost:5054/map1
// app.Map("/map1", HandleMap1);
// static void HandleMap1(IApplicationBuilder applicationBuilder)
// {
//     applicationBuilder.Run(async context =>
//     {
//         await context.Response.WriteAsync("Hello world from map");
//         //await context.Response.WriteAsync(context.Request.ToString());
//     });
// }

// 3 way to create middleware ( use )
// app.Use(async (context, next) =>
// {
//     await next.Invoke();
// });

app.Run();
