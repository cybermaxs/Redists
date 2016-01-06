using System;
using Redists.Extensions;

namespace Redists
{
    /// <summary>
    /// Basic structure that represent a couple Key-Value
    /// </summary>
    public struct DataPoint
    {
        /// <summary>
        /// An Empty DataPoint.
        /// </summary>
        public static readonly DataPoint Empty = new DataPoint();
        /// <summary>
        /// TimeStamp of the DataPoint.
        /// </summary>
        public long timestamp;
        /// <summary>
        /// Value of the DataPoint.
        /// </summary>
        public readonly long value;

        /// <summary>
        /// Instancite a new DataPoint using a timestamp and a value.
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="value"></param>
        public DataPoint(long timestamp, long value)
        {
            this.timestamp = timestamp;
            this.value = value;
        }

        /// <summary>
        /// Instancite a new DataPoint using a DateTime and a value.
        /// </summary>
        /// <param name="at"></param>
        /// <param name="value"></param>
        public DataPoint(DateTime at, long value)
        {
            timestamp = at.ToTimestamp();
            this.value = value;
        }

        /// <summary>
        /// Get DateTime for the current timestamp.
        /// </summary>
        /// <returns></returns>
        public DateTime GetOriginalDateTime()
        {
            return timestamp.ToDateTime();
        }

        internal void Normalize(long factor)
        {
            timestamp = timestamp.Normalize(factor);
        }

        public static bool operator ==(DataPoint x, DataPoint y)
        {
            return x.timestamp == y.timestamp && x.value == y.value;
        }
        public static bool operator !=(DataPoint x, DataPoint y)
        {
            return !(x == y);
        }

        #region overrides
        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj"> An object to compare with this instance.</param>
        /// <returns>true if obj is an instance of an System.Int64 and equals the value of this instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var r = (DataPoint)obj;
            return (timestamp == r.timestamp) && (value == r.value);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns> A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return timestamp.GetHashCode() + value.GetHashCode();
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return timestamp.ToString() + ":" + value.ToString();
        }
        #endregion
    }
}
