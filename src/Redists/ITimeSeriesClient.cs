using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redists
{
    public interface ITimeSeriesClient
    {
        Task AddAsync(DateTime at, long value);
        Task AddAsync(params DataPoint[] dataPoints);
        Task<DataPoint[]> AllAsync(DateTime at, DateTime? to = null);
    }
}
