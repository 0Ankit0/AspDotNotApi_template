using API_TEMPLATE.CustomClass;
using API_TEMPLATE.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API_TEMPLATE.Configuration
{
    public class JwtConfiguration
    {
        private readonly IConfiguration _configuration;

        public JwtConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configure JWT settings
            services.Configure<JwtSettings>(_configuration.GetSection("JwtSettings"));

            // Inject JwtSettings using IOptions
            services.AddSingleton<IJwtAuth>(serviceProvider =>
            {
                var jwtSettings = serviceProvider.GetRequiredService<IOptions<JwtSettings>>();
                return new JwtAuth(jwtSettings);
            });

            // Configure JWT authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var serviceProvider = services.BuildServiceProvider();
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
        }
    }
}
