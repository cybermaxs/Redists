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
        /// Use fixed size for each data point ?
        /// default is 29 (timestamp in ms + delimiter + long)
        /// </summary>
        public bool UseFixedSize { get; private set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="keyNormalizationFactor">Key normalization factor (ms)</param>
        /// <param name="dataPointNormalizationFactor">Data point normalization factor (ms)</param>
        /// <param name="useFixedSize">Fixed or dynamic dataPoint size</param>
        /// <param name="keyTtl">Key Ttl value</param>
        public TimeSeriesOptions(long keyNormalizationFactor, long dataPointNormalizationFactor, bool useFixedSize, TimeSpan? keyTtl)
        {
            if (keyNormalizationFactor < dataPointNormalizationFactor)
                throw new InvalidOperationException("keyNormalizationFactor should be greater than dataPointNormalizationFactor");

            if (keyTtl.HasValue && keyTtl.Value.TotalMilliseconds < keyNormalizationFactor)
                throw new InvalidOperationException("keyTtl should be greater than keyNormalizationFactor");

            this.KeyNormFactor = keyNormalizationFactor;
            this.KeyTtl = keyTtl;
            this.DataPointNormFactor = dataPointNormalizationFactor;
            this.UseFixedSize = useFixedSize;
        }
    }
}
