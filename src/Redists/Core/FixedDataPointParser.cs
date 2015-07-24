using System;
using System.Linq;
using Redists.Extensions;
using System.Collections.Generic;

namespace Redists.Core
{
    internal class FixedDataPointParser : IDataPointParser
    {
        public static int FixedKeyLength = 13;
        public static int FixedValueLength = 19;

        static FixedDataPointParser()
        {
            FixedKeyLength = DateTime.UtcNow.ToTimestamp().ToString().Length;
            FixedValueLength = long.MaxValue.ToString().Length;
        }

        #region publics
        public DataPoint[] ParseRawString(string raw)
        {
            return ParseFixed(raw);
        }
        public DataPoint Deserialize(string rawDataPoint)
        {
            var parts = rawDataPoint.Split(Constants.IntraDelimiter);
            long ts;
            long value;
            long.TryParse(parts[0], out ts);
            long.TryParse(parts[1], out value);

            return new DataPoint(ts, value);
        }

        public string Serialize(params DataPoint[] dataPoint)
        {
            return string.Join(Constants.InterDelimiter.ToString(), dataPoint.Select(r=>this.SerializeInternal(r)).ToArray());
        }
        #endregion

        #region privates
        private DataPoint[] ParseFixed(string raw)
        {
            var fixedSize = FixedKeyLength + FixedValueLength + 1;
            var nbItems = raw.Length / (fixedSize + 1);
            var results = new DataPoint[nbItems];

            int current = 0;
            while (current != raw.Length)
            {
                DataPoint dataPoint = this.Deserialize(raw.Substring(current, fixedSize));
                results[current / (fixedSize + 1)] = dataPoint;
                current += (fixedSize + 1);
            }
            return results;
        }

        private string SerializeInternal(DataPoint dataPoint)
        {
            if (dataPoint == DataPoint.Empty)
                return string.Empty;

            var stringTs = dataPoint.ts.ToString().PadLeft(FixedKeyLength, Constants.PadChar);
            var stringValue = dataPoint.value.ToString().PadLeft(FixedValueLength, Constants.PadChar);

            return stringTs + Constants.IntraDelimiter + stringValue;
        }
        #endregion

    }
}
