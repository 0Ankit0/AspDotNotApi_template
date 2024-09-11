using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Api_Template.Classes;
using System.Text;
using AspNetCoreRateLimit;
using Newtonsoft.Json;
using Api_Template.Models;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


// Read the connection string
var BaseAddress = builder.Configuration.GetConnectionString("DefaultConnection");


//Add datahandler as transient
builder.Services.AddTransient<IDataHandler>(ServiceProvider =>
{
    // Read the connection string
    var connectionString = builder.Configuration.GetConnectionString("BaseAddress");
    return new DatabaseHelper(connectionString);
});

// Configure JWT settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Inject JwtSettings using IOptions
builder.Services.AddSingleton<IJwtAuth>(serviceProvider =>
{
    var jwtSettings = serviceProvider.GetRequiredService<IOptions<JwtSettings>>();
    return new JwtAuth(jwtSettings);
});

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var serviceProvider = builder.Services.BuildServiceProvider();
        var jwtSettings = serviceProvider.GetRequiredService<IOptions<JwtSettings>>().Value;
        var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true, // Validates token expiry
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                var userIdClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
                var usernameClaim = claimsIdentity?.FindFirst(ClaimTypes.Name);

                if (userIdClaim != null && usernameClaim != null)
                {
                    var user = new AuthenticatedUser
                    {
                        UserId = userIdClaim.Value,
                        Username = usernameClaim.Value
                    };

                    // Add the user object to the HttpContext items
                    context.HttpContext.Items["User"] = user;
                }

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                // Log the exception
                context.NoResult();
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync(context.Exception.ToString());
            }
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

builder.Services.AddSignalR();

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
builder.Services.AddSingleton(new ConcurrentDictionary<string, string>());

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
app.UseCors();

app.MapControllers();
app.MapHub<MessageHub>("/messagehub");

app.Run();