//dotnet add package Swashbuckle.AspNetCore.Annotations
//to add data annotations for the api endpoint
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace API_TEMPLATE.Configuration
{
    public class SwaggerConfiguration
    {
        private readonly IConfiguration _configuration;

        public SwaggerConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configure API versioning
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "VV";
                options.SubstituteApiVersionInUrl = true;
            });

            // Add Swagger and configure SwaggerGen
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                // Use the IServiceProvider to resolve IApiVersionDescriptionProvider
                var serviceProvider = services.BuildServiceProvider();
                var apiVersionDescriptionProvider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

                //To add data annotations using washbuckle.AspNetCore.Annotations
                options.EnableAnnotations();

                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }

                // Add Bearer Token Authentication support
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }

        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = "My API",
                Version = description.ApiVersion.ToString(),
                Description = $"API Description for version {description.ApiVersion}"
            };

            if (description.IsDeprecated)
            {
                info.Description += " (This version is deprecated)";
            }

            return info;
        }
    }
}
