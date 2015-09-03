using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redists
{
    public interface ITimeSeriesClient
    {
        Task<long> AddAsync(DateTime at, long value);
        Task<long> AddAsync(params DataPoint[] dataPoints);
        Task<DataPoint[]> AllSinceAsync(DateTime at);
        Task<DataPoint[]> RangeAsync(DateTime from, DateTime to);
    }
}
