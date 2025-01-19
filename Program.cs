using Asp.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using NurseryMart.IRepository;
using NurseryMart.MiddleWares;
using NurseryMart.Repositories;
using NurseryMart.Services.Abstraction;
using NurseryMart.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddDbContext<NurseryMartDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NurseryMart")));

builder.Services.AddScoped<IAuth, AuthRepository>(); 
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.

 app.UseSwagger();
 app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthorization();
//app.UseWhen(_ => !_.Request.Path.StartsWithSegments("/api/public"), _app => _app.UseMiddleware<JwtMiddleware>());
app.MapControllers();

app.Run();
