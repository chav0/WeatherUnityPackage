using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace WeatherServices
{
    public class WeatherServiceManager
    {
        private readonly List<IWeatherService> _services = new();

        public void AddService(IWeatherService service)
        {
            _services.Add(service);
        }
        
        public async Task<IReadOnlyList<WeatherData>> GetWeather(float timeout, CancellationToken cancellationToken)
        {
            if (!Input.location.isEnabledByUser)
                throw new Exception("Location is disabled by user!");
            
            using var timeoutCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(timeout));
            var timeoutCancellationToken = timeoutCancellationTokenSource.Token;

            var (latitude, longitude) = await GetLocation(timeoutCancellationToken);
            return await GetWeather(latitude, longitude, timeoutCancellationToken);
        }

        public Task<IReadOnlyList<WeatherData>> GetWeather(double latitude, double longitude, float timeout, CancellationToken cancellationToken)
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

        private async Task<(double, double)> GetLocation(CancellationToken cancellationToken)
        {
            Input.location.Start();
            
            while (Input.location.status == LocationServiceStatus.Initializing)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Input.location.Stop();
                    throw new OperationCanceledException(cancellationToken);
                }

                await Task.Yield();
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Input.location.Stop();
                throw new Exception("Getting location is failed!");
            }

            var locationInfo = Input.location.lastData;
            var latitude = locationInfo.latitude;
            var longitude = locationInfo.longitude;

            Input.location.Stop();

            return (latitude, longitude);
        }
    }
}