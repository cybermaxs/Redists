using System;

namespace Redists
{
    public class TimeSerieSettings
    {
        /// <summary>
        /// Parition factor for Serie Key.
        /// </summary>
        public long SerieNormalizeFactor { get; set; }
        /// <summary>
        /// Normalization factor for each data point.
        /// </summary>
        public long RecordNormalizeFactor { get; set; }
        /// <summary>
        /// Use fixed size for each record ?
        /// default is 29 (timestamp in ms + delimiter + long)
        /// </summary>
        public bool UseFixedRecordSize { get; set; }
        public TimeSerieSettings(long serieNormalizeFactor = 3600, long recordNormalizeFactor = 1, bool useFixedRecordSize = false)
        {
            if (serieNormalizeFactor < recordNormalizeFactor)
                throw new InvalidOperationException("serieNormalizeFactor should be greater than recordNormalizeFactor");

            this.SerieNormalizeFactor = serieNormalizeFactor; ;
            this.RecordNormalizeFactor = recordNormalizeFactor;
            this.UseFixedRecordSize = useFixedRecordSize;
        }
    }
}
