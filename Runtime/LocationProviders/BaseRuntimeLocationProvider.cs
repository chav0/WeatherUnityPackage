using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace WeatherServices
{
    public abstract class BaseRuntimeLocationProvider : ILocationProvider
    {
        public virtual async Task<(double, double)> GetLocation(CancellationToken cancellationToken)
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
            
            if (!Input.location.isEnabledByUser)
                throw new Exception("Location is disabled by user!");

            var locationInfo = Input.location.lastData;
            var latitude = locationInfo.latitude;
            var longitude = locationInfo.longitude;

            Input.location.Stop();

            return (latitude, longitude);
        }
    }
}