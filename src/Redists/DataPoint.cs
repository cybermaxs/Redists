using System;
using Redists.Extensions;

namespace Redists
{
    /// <summary>
    /// Basic structure that represent a couple Key-Value
    /// </summary>
    public struct DataPoint
    {
        public static DataPoint Empty = new DataPoint();
        public long ts;
        public long value;

        public DataPoint(long timeStamp, long value)
        {
            this.ts = timeStamp;
            this.value = value;
        }

        public DataPoint(DateTime at, long value)
        {
            this.ts = at.ToTimestamp();
            this.value = value;
        }

        public void Normalize(long factor)
        {
            this.ts=this.ts.Normalize(factor);
        }

        public static bool operator ==(DataPoint x, DataPoint y)
        {
            return x.ts == y.ts && x.value == y.value;
        }
        public static bool operator !=(DataPoint x, DataPoint y)
        {
            return !(x == y);
        }

        #region overrides
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            DataPoint r = (DataPoint)obj;
            return (ts == r.ts) && (value == r.value);
        }

        public override int GetHashCode()
        {
            return ts.GetHashCode() + value.GetHashCode();
        }
        public override string ToString()
        {
            return this.ts + ":" + this.value.ToString();
        }
        #endregion
    }
}
