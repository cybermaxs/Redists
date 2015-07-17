using System;

namespace Redists
{
    public class Settings
    {
        /// <summary>
        /// Partition key factor for a Serie (Milliseconds).
        /// Used to compute the redis key name.
        /// </summary>
        public long SerieNormFactor { get; private set; }
        /// <summary>
        /// Normalization factor for each data point (Milliseconds).
        /// </summary>
        public long RecordNormFactor { get; private set; }
        /// <summary>
        /// Use fixed size for each record ?
        /// default is 29 (timestamp in ms + delimiter + long)
        /// </summary>
        public bool UseFixedRecordSize { get; private set; }
        public Settings(long serieNormalizeFactor = 3600, long recordNormalizeFactor = 1, bool useFixedRecordSize = false)
        {
            if (serieNormalizeFactor < recordNormalizeFactor)
                throw new InvalidOperationException("serieNormalizeFactor should be greater than recordNormalizeFactor");

            this.SerieNormFactor = serieNormalizeFactor;
            this.RecordNormFactor = recordNormalizeFactor;
            this.UseFixedRecordSize = useFixedRecordSize;
        }
    }
}
