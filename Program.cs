
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceApp_backend.Classes;
using System.Text;
using AspNetCoreRateLimit;
using Newtonsoft.Json;
using ServiceApp_backend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Retrieve the connection string from the configuration.
DatabaseSettings.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");


// Configure JWT authentication.
var jwtAuth = new JwtAuth("aBcDeFgHiJkLmNoPqRsTuVwXyZ0123456789!@#$%^&*()", "Ankit", "AllUsers");
builder.Services.AddSingleton(jwtAuth);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtAuth.Issuer,
            ValidAudience = jwtAuth.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtAuth.SecretKey))
        };
    });

//Configure IP rate limiting services
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>(); // Add processing strategy


// Configure Swagger/OpenAPI.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseIpRateLimiting();

app.UseAuthentication();

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == 401)
    {
        response.ContentType = "application/json";
        var rm = new ResponseModel
        {
            message = "Sorry You are not Authorized",
            status = 401,
            data = new { tokenNo = "" }
        };
        await response.WriteAsync(JsonConvert.SerializeObject(rm));
    }
});

app.UseAuthorization();

app.MapControllers();

app.Run();