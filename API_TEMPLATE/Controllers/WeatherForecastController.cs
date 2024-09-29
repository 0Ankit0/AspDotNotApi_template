using API_TEMPLATE.Filter;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace API_TEMPLATE.Controllers
{
    [ApiController]
    [Route("api/{version:apiVersion}/WeatherForecast")]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [MapToApiVersion("1.0")]
        [SimpleRateLimit(maxRequests: 5, seconds: 60)] // Allow 5 requests per 60 seconds
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Message = "This is from version 1.0"
            })
            .ToArray();
        }


        [HttpGet(Name = "GetWeatherForecast")]
        [MapToApiVersion("2.0")]
        [SimpleRateLimit(maxRequests: 5, seconds: 60)] // Allow 5 requests per 60 seconds
        public IEnumerable<WeatherForecast> Get2()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Message = "This is from version 2.0"
            })
            .ToArray();
        }
    }

}
