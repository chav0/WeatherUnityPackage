using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace WeatherServices
{
    public abstract class BaseWeatherService<T> where T : struct
    {
        protected async Task<WeatherData> GetWeatherRequest(string url, CancellationToken cancellationToken)
        {
            using var request = UnityWebRequest.Get(url);
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    request.Abort();
                    throw new OperationCanceledException(cancellationToken);
                }

                await Task.Yield();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception($"Web request error: {request.error}");
            }

            var jsonResult = request.downloadHandler.text;
            Debug.Log(jsonResult);
            return ParseWeatherData(jsonResult);
        }

        private WeatherData ParseWeatherData(string json)
        {
            var weatherResponse = JsonUtility.FromJson<T>(json);
            return OnParseWeatherData(weatherResponse);
        }

        protected abstract WeatherData OnParseWeatherData(T weatherResponse);
    }
}