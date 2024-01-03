using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace WeatherServices
{
#if UNITY_ANDROID
        public class AndroidLocationProvider : BaseRuntimeLocationProvider
    {
        private const string CoarseLocation = UnityEngine.Android.Permission.CoarseLocation;
        
        public override Task<(double, double)> GetLocation(CancellationToken cancellationToken)
        {
            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(CoarseLocation))
            {
                // get permission
            }
            
            if (!Input.location.isEnabledByUser)
                throw new Exception("Location is disabled by user!");
            
            return base.GetLocation(cancellationToken);
        }
    }
#endif
}