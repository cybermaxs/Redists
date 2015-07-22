using System;

namespace Redists.Configuration
{
    /// <summary>
    /// Various settings to configure a timeseries.
    /// </summary>
    public class TimeSerieSettings
    {
        /// <summary>
        /// Partition key factor for a Serie (Milliseconds).
        /// Used to compute the redis key name.
        /// </summary>
        public long KeyNormFactor { get; private set; }
        /// <summary>
        /// TimeToLive for a Serie (Milliseconds).
        /// </summary>
        public TimeSpan? Ttl { get; private set; }
        /// <summary>
        /// Normalization factor for each data point (Milliseconds).
        /// </summary>
        public long RecordNormFactor { get; private set; }
        /// <summary>
        /// Use fixed size for each record ?
        /// default is 29 (timestamp in ms + delimiter + long)
        /// </summary>
        public bool UseFixedRecordSize { get; private set; }

        public TimeSerieSettings(long serieNormalizeFactor = 3600*1000, long recordNormalizeFactor = 1000, bool useFixedRecordSize = false, TimeSpan? serieTtl = null)
        {
            if (serieNormalizeFactor < recordNormalizeFactor)
                throw new InvalidOperationException("serieNormalizeFactor should be greater than recordNormalizeFactor");    

            this.KeyNormFactor = serieNormalizeFactor;
            this.Ttl = serieTtl;
            this.RecordNormFactor = recordNormalizeFactor;
            this.UseFixedRecordSize = useFixedRecordSize;
        }
    }
}
