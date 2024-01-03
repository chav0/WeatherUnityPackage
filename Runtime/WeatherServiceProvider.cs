using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace WeatherServices
{
    public class WeatherServiceProvider : IWeatherProvider
    {
        private readonly List<IWeatherService> _services = new();

#if UNITY_EDITOR
        private readonly ILocationProvider _locationProvider = new EditorLocationProvider();     
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        private readonly ILocationProvider _locationProvider = new AndroidLocationProvider();   
#endif

#if UNITY_IOS && !UNITY_EDITOR
        private readonly ILocationProvider _locationProvider = new IOSLocationProvider();   
#endif


        void IWeatherProvider.AddService(IWeatherService service) => _services.Add(service);

        async Task<IReadOnlyList<WeatherData>> IWeatherProvider.GetWeather(float timeout, CancellationToken cancellationToken)
        {
            using var timeoutCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(timeout));
            var timeoutCancellationToken = timeoutCancellationTokenSource.Token;

            var (latitude, longitude) = await _locationProvider.GetLocation(timeoutCancellationToken);
            return await GetWeather(latitude, longitude, timeoutCancellationToken);
        }

        Task<IReadOnlyList<WeatherData>> IWeatherProvider.GetWeather(double latitude, double longitude, float timeout, CancellationToken cancellationToken)
        {
            using var timeoutCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(timeout));
            var timeoutCancellationToken = timeoutCancellationTokenSource.Token;

            return GetWeather(latitude, longitude, timeoutCancellationToken);
        }

        private async Task<IReadOnlyList<WeatherData>> GetWeather(double latitude, double longitude, CancellationToken cancellationToken)
        {
            var weatherData = new List<WeatherData>(_services.Count);
            foreach (var service in _services)
            {
                var weatherTask = service.GetWeather(latitude, longitude, cancellationToken);
                weatherData.Add(await weatherTask);
            }

            return weatherData;
        }
    }
}