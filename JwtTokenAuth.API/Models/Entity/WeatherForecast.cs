namespace JwtTokenAuth.API.Models.Entity
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }  //degrees Celsius

        public string Summary { get; set; } = string.Empty;
    }
}
