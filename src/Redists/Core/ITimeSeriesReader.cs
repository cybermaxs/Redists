using System.Threading.Tasks;

namespace Redists.Core
{
    interface ITimeSeriesReader
    {
        Task<DataPoint[]> ReadAllAsync(string redisKey);
    }
}
