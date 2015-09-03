using System;
using System.Threading.Tasks;

namespace Redists.Core
{
    public interface ITimeSeriesWriter
    {
        Task<long> AppendAsync(string redisKey, DataPoint[] dataPoints);
    }
}
