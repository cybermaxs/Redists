using System.Threading.Tasks;

namespace Redists.Core
{
    public interface ITimeSeriesReader
    {
        Task<DataPoint[]> ReadAllAsync(string redisKey);
    }
}
