using System;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherServices
{
    public class OpenMeteoWeatherService : BaseWeatherService<OpenMeteoWeatherService.WeatherResponse>, IWeatherService
    {
        private const string BaseUrl = "https://api.open-meteo.com/v1/forecast";

        public Task<WeatherData> GetWeather(double latitude, double longitude, CancellationToken cancellationToken)
        {
            var url = $"{BaseUrl}?latitude={latitude}&longitude={longitude}&current=temperature_2m,relative_humidity_2m,apparent_temperature,is_day,weather_code,surface_pressure,wind_speed_10m,wind_direction_10m&forecast_days=1";
            return GetWeatherRequest(url, cancellationToken);
        }
        
        [Serializable]
        public struct WeatherResponse
        {
            public CurrentWeatherData current;
        }

        [Serializable]
        public struct CurrentWeatherData
        {
            public double temperature_2m;
            public int weather_code;
            public int relative_humidity_2m;
            public float surface_pressure;
            public int is_day;
            public double wind_speed_10m;
            public int wind_direction_10m;
        }

        protected override WeatherData OnParseWeatherData(WeatherResponse weatherResponse)
        {
            var weatherType = weatherResponse.current.weather_code switch
            {
                0 => WeatherType.Clear,
                >= 1 and <= 3 => WeatherType.Clouds,
                45 or 48 => WeatherType.Fog,
                >= 51 and <= 59 => WeatherType.Drizzle,
                >= 61 and <= 69 or 81 or 82 or 83 => WeatherType.Rain,
                >= 71 and <= 79 or 85 or 86 => WeatherType.Snow,
                >= 91 and <= 99 => WeatherType.Thunderstorm,
                _ => WeatherType.None
            };
            
            var weatherData = new WeatherData
            {
                Type = weatherType,
                Temperature = weatherResponse.current.temperature_2m,
                Humidity = weatherResponse.current.relative_humidity_2m,
                Pressure = weatherResponse.current.surface_pressure,
                WindSpeed = weatherResponse.current.wind_speed_10m,
                WindDirection = weatherResponse.current.wind_direction_10m,
                IsDay = weatherResponse.current.is_day == 1
            };

            return weatherData;
        }
    }
}