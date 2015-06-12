using System;

namespace Redists
{
    /// <summary>
    /// Basic structure that represent a couple Key-Value
    /// </summary>
    public struct Record
    {
        public static Record Empty = new Record();
        public long ts;
        public long value;

        public Record(long timeStamp, long value)
        {
            this.ts = timeStamp;
            this.value = value;
        }

        public static bool operator ==(Record x, Record y)
        {
            return x.ts == y.ts && x.value == y.value;
        }
        public static bool operator !=(Record x, Record y)
        {
            return !(x == y);
        }

        #region overrides
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Record r = (Record)obj;
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
