using System;

namespace Redists.Configuration
{
    /// <summary>
    /// Various settings to configure a timeseries.
    /// </summary>
    public class TimeSeriesOptions
    {
        /// <summary>
        /// Partition key factor for a Serie (Milliseconds).
        /// Used to compute the redis key name.
        /// </summary>
        public long KeyNormFactor { get; private set; }
        /// <summary>
        /// TimeToLive for a TimeSeries (Milliseconds).
        /// </summary>
        public TimeSpan? KeyTtl { get; private set; }
        /// <summary>
        /// Normalization factor for each data point (Milliseconds).
        /// </summary>
        public long DataPointNormFactor { get; private set; }

        /// <summary>
        /// Prefix added to the TimeSeries name.
        /// </summary>
        public string Prefix { get; private set; }
        /// <summary>
        /// Delimiter between each data points
        /// </summary>
        public char Delimiter { get; private set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="keyNormalizationFactor">Key normalization factor (ms)</param>
        /// <param name="dataPointNormalizationFactor">Data point normalization factor (ms)</param>
        /// <param name="keyTtl">Key Ttl value</param>
        /// <param name="prefix">Prefix added to the TimeSeries name</param>
        /// <param name="delimiter">Delimiter between each data points</param>
        public TimeSeriesOptions(long keyNormalizationFactor, long dataPointNormalizationFactor, TimeSpan? keyTtl, string prefix = Constants.DefaultTsPrefix, char delimiter = Constants.DefaultInterDelimiterChar)
        {
            if (keyNormalizationFactor < dataPointNormalizationFactor)
                throw new InvalidOperationException("keyNormalizationFactor should be greater than dataPointNormalizationFactor");

            if (keyTtl.HasValue && keyTtl.Value.TotalMilliseconds < keyNormalizationFactor)
                throw new InvalidOperationException("keyTtl should be greater than keyNormalizationFactor");

            KeyNormFactor = keyNormalizationFactor;
            KeyTtl = keyTtl;
            DataPointNormFactor = dataPointNormalizationFactor;
            Prefix = prefix;
            Delimiter = delimiter;
        }
    }
}
