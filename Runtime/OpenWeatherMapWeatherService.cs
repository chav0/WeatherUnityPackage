using System;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherServices
{
    public class OpenWeatherMapWeatherService : BaseWeatherService<OpenWeatherMapWeatherService.WeatherResponse>, IWeatherService
    {
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";

        public Task<WeatherData> GetWeather(double latitude, double longitude, CancellationToken cancellationToken)
        {
            var url = $"{BaseUrl}?lat={latitude}&lon={longitude}&units=metric&appid=906ea498ebc3dc7a609b9f95c4e56338";
            return GetWeatherRequest(url, cancellationToken);
        }
        
        [Serializable]
        public struct WeatherResponse
        {
            public CommonWeatherData[] weather;
            public MainWeatherData main;
            public int visibility;
            public Wind wind;
            public Clouds clouds;
            public long dt;
            public SystemData sys;
        }
        
        [Serializable]
        public struct CommonWeatherData
        {
            public int id;
            public string main;
            public string description;
            public string icon;
        }

        [Serializable]
        public struct MainWeatherData
        {
            public double temp;
            public double feels_like;
            public double temp_min;
            public double temp_max;
            public int pressure;
            public int humidity;
        }

        [Serializable]
        public struct Wind
        {
            public double speed;
            public int deg;
        }

        [Serializable]
        public struct Clouds
        {
            public int all;
        }

        [Serializable]
        public struct SystemData
        {
            public int type;
            public long id;
            public string country;
            public long sunrise;
            public long sunset;
        }

        protected override WeatherData OnParseWeatherData(WeatherResponse weatherResponse)
        {
            Enum.TryParse<WeatherType>(weatherResponse.weather[0].main, out var weatherType);
            
            var weather = new WeatherData
            {
                Type = weatherType,
                Temperature = weatherResponse.main.temp,
                Humidity = weatherResponse.main.humidity,
                Pressure = weatherResponse.main.pressure,
                Visibility = weatherResponse.visibility,
                WindSpeed = weatherResponse.wind.speed,
                WindDirection = weatherResponse.wind.deg,
                IsDay = weatherResponse.dt >= weatherResponse.sys.sunrise && 
                        weatherResponse.dt < weatherResponse.sys.sunset
            };

            return weather;
        }
    }
}