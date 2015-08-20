using System;
using System.Threading.Tasks;

namespace Redists.Core
{
    interface ITimeSeriesWriter
    {
        Task<long> AppendAsync(string redisKey, DataPoint[] dataPoints);
    }
}
