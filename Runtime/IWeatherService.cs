using System.Threading;
using System.Threading.Tasks;

namespace WeatherServices
{
    public interface IWeatherService
    {
        Task<WeatherData> GetWeather(double latitude, double longitude, CancellationToken cancellationToken);
    }
}