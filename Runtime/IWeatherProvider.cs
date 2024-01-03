using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherServices
{
    public interface IWeatherProvider
    {
        void AddService(IWeatherService service);
        Task<IReadOnlyList<WeatherData>> GetWeather(float timeout, CancellationToken cancellationToken);
        Task<IReadOnlyList<WeatherData>> GetWeather(double latitude, double longitude, float timeout, CancellationToken cancellationToken);
    }
}