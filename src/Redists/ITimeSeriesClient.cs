using System;
using System.Threading.Tasks;

namespace Redists
{
    /// <summary>
    /// Defines Basic opterations on a TimeSeries created  by TimeSeriesFactory.
    /// </summary>
    public interface ITimeSeriesClient
    {
        /// <summary>
        /// Add a new value to the time series.
        /// </summary>
        /// <param name="at">Timestamp of the data point (when)</param>
        /// <param name="value">Value of the data point</param>
        /// <returns>Number of Bytes written.</returns>
        Task<long> AddAsync(DateTime at, long value);
        /// <summary>
        /// Add a batch of Data Points.
        /// </summary>
        /// <returns>Number of Bytes written.</returns>
        Task<long> AddAsync(params DataPoint[] dataPoints);
        /// <summary>
        /// Get all DataPoints between at and now. Shortcut for Range(at, DateTime.UtcNow).
        /// </summary>
        /// <param name="at"></param>
        /// <returns>Array of DataPoints</returns>
        Task<DataPoint[]> AllSinceAsync(DateTime at);
        /// <summary>
        /// Get all DataPoints between at and to for this client..
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>Array of DataPoints</returns>
        Task<DataPoint[]> RangeAsync(DateTime from, DateTime to);
    }
}
