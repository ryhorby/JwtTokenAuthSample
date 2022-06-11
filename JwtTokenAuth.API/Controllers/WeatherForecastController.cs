using JwtTokenAuth.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JwtTokenAuth.API.Controllers
{
    [ApiController]
    [Route("controller")]
    public class WeatherForecastController : Controller
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cold", "Mild", "Warm", "Balmy", "Hot"
        };

        [HttpGet("getWeatherForecastForTwoWeeks"), Authorize(Roles="Admin")]
        public IEnumerable<WeatherForecast> GetForOneTwoWeeks()
        {
            List<WeatherForecast> forecasts = new List<WeatherForecast>();

            for (int i = 0; i < 14; i++)
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



        [HttpGet("getWeatherForecastForWeek"), Authorize]
        public IEnumerable<WeatherForecast> GetForOneWeek()
        {
            List<WeatherForecast> forecasts = new List<WeatherForecast>();
            
            for (int i = 0; i < 7; i++)
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


        [HttpGet("getWeatherForecastForTwoDays"), AllowAnonymous]
        public IEnumerable<WeatherForecast> GetForTwoDays()
        {
            List<WeatherForecast> forecasts = new List<WeatherForecast>();

            for (int i = 0; i < 2; i++)
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
