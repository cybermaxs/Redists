using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redists
{
    public interface ITimeSeriesClient
    {
        Task AddAsync(long value, DateTime at);
        Task AddAsync(IEnumerable<DataPoint> dataPoints);
        Task<DataPoint[]> AllAsync(DateTime at, DateTime? to = null);
    }
}
