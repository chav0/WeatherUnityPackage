using System.Threading;
using System.Threading.Tasks;

namespace WeatherServices
{
    public class EditorLocationProvider : ILocationProvider
    {
        Task<(double, double)> ILocationProvider.GetLocation(CancellationToken cancellationToken)
        {
            return Task.FromResult((44.815901943749715, 20.46072437380213));
        }
    }
}