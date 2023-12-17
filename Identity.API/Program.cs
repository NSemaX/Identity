using Identity.API.Application;
using Identity.API.Infrastructure;
using Identity.API.Infrastructure.Repositories;
using Identity.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
   {
       options.ExampleFilters();
   });
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

builder.Services.Configure<APISettings>(builder.Configuration.GetSection("APISettings"))
                .Configure<JwtAuthentication>(builder.Configuration.GetSection("JwtAuthentication"));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddOptions();

string connectionString= builder.Configuration.GetSection("ConnectionString").Value;


builder.Services.AddTransient<ITokenRepository>(s => new TokenRepository(connectionString));
builder.Services.AddTransient<IIdentityOperations, IdentityOperations>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
