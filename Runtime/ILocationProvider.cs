using System.Threading;
using System.Threading.Tasks;

namespace WeatherServices
{
    public interface ILocationProvider
    {
        Task<(double, double)> GetLocation(CancellationToken cancellationToken); 
    }
}