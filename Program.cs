using Asp.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using NurseryMart.MiddleWares;
using NurseryMart.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.TryAddSingleton<IApiDescriptionGroupCollectionProvider, ApiDescriptionGroupCollectionProvider>();
builder.Services.TryAddEnumerable(
       ServiceDescriptor.Transient<IApiDescriptionProvider, DefaultApiDescriptionProvider>());
builder.Services.AddTransient<SqlConnectionFactory>();
//builder.Services.AddSingleton(_scrope =>
//{
//    var connectionString = builder.Configuration["ConnectionStrings:MongoDb"];
//    var mongoClient = new MongoClient(connectionString);
//    return mongoClient.GetDatabase(builder.Configuration["Databases:MongoDb"]);
//});

var app = builder.Build();

// Configure the HTTP request pipeline.

 app.UseSwagger();
 app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthorization();
app.UseWhen(_ => !_.Request.Path.StartsWithSegments("/api/public"), _app => _app.UseMiddleware<JwtMiddleware>());
app.MapControllers();

app.Run();
