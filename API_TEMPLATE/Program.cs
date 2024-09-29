using API_TEMPLATE.Configuration;
using API_TEMPLATE.CustomClass;
using API_TEMPLATE.Data;
using API_TEMPLATE.Models;
using API_TEMPLATE.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Instantiate the swagger configuration class
var swaggerConfig = new SwaggerConfiguration(builder.Configuration);

// Configure services using the instance
swaggerConfig.ConfigureServices(builder.Services);

// Configure Entity Framework Core with SQL Server more info https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/intro?view=aspnetcore-8.0
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BaseAddress")));



//Add SignalR services more info https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction?view=aspnetcore-8.0
builder.Services.AddSignalR();

//To Add Cors policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder
                    .WithOrigins("http://127.0.0.1:5500")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

//Add the custom AsymmetricCryptography as transient
builder.Services.AddSingleton<ICustomCryptography, AssymetricCryptography>();

//Add a dictionary as a singleton to map the connection id with the token in hub
builder.Services.AddSingleton(new ConcurrentDictionary<string, MapToHubId>());


// Read the connection string
var BaseAddress = builder.Configuration.GetConnectionString("BaseAddress");

//Add imemoryCache along with memory cache
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICustomMemoryCache, CustomMemoryCache>();


//Add datahandler as transient
builder.Services.AddTransient<IDataHandler>(ServiceProvider =>
{
    // Read the connection string
    var connectionString = builder.Configuration.GetConnectionString("BaseAddress");
    return new DatabaseHelper(connectionString);
});

// Instantiate the JWT configuration class
var jwtConfig = new JwtConfiguration(builder.Configuration);

// Configure services using the instance
jwtConfig.ConfigureServices(builder.Services);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
    options =>
    {
        var descriptions = app.DescribeApiVersions();

        foreach (var description in descriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

//To use authentication
app.UseAuthentication();

app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.MapHub<MessageHub>("/messagehub");

app.Run();
