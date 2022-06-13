using JwtTokenAuth.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JwtTokenAuth.API.Controllers
{
    [ApiController]
    [Route("controller")]
    public class WeatherForecastController : Controller
    {
        const int NUMBER_OF_DAYS_FOR_ALL = 2;
        const int NUMBER_OF_DAYS_FOR_AUTH = 7;
        const int NUMBER_OF_DAYS_FOR_ADMIN = 14;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cold", "Mild", "Warm", "Balmy", "Hot"
        };

        
        [HttpGet("getWeatherForecastForTwoWeeks"), Authorize(Roles="Admin")]
        public IEnumerable<WeatherForecast> GetForOneTwoWeeks()
        {
            return GetWeatherForecast(NUMBER_OF_DAYS_FOR_ADMIN);
        }

        [HttpGet("getWeatherForecastForWeek"), Authorize]
        public IEnumerable<WeatherForecast> GetForOneWeek()
        {
            return GetWeatherForecast(NUMBER_OF_DAYS_FOR_AUTH);
        }

        [HttpGet("getWeatherForecastForTwoDays"), AllowAnonymous]
        public IEnumerable<WeatherForecast> GetForTwoDays()
        {
            return GetWeatherForecast(NUMBER_OF_DAYS_FOR_ALL);
        }

        private IEnumerable<WeatherForecast> GetWeatherForecast(int numberOfDays)
        {
            List<WeatherForecast> forecasts = new List<WeatherForecast>();

            for (int i = 0; i < numberOfDays; i++)
            {
                forecasts.Add(new WeatherForecast()
                {
                    Date = DateTime.Now.AddDays(i + 1),
                    TemperatureC = Random.Shared.Next(-30, 30),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                });
            }

            return forecasts;
        }
    }
}
