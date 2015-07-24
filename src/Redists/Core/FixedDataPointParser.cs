using System;
using System.Linq;
using Redists.Extensions;
using System.Collections.Generic;

namespace Redists.Core
{
    internal class FixedDataPointParser : IDataPointParser
    {
        public const int KeyLength = 13;
        public const string KeyFormat = "D13";
        public const int ValueLength = 19;
        public const string ValueFormat = "D19";

        #region publics
        public DataPoint[] ParseRawString(string raw)
        {
            return ParseFixed(raw);
        }
        public DataPoint Deserialize(string rawDataPoint)
        {
            var parts = rawDataPoint.Split(new string[] { Constants.IntraDelimiter }, StringSplitOptions.RemoveEmptyEntries);
            long ts;
            long value;
            long.TryParse(parts[0], out ts);
            long.TryParse(parts[1], out value);

            return new DataPoint(ts, value);
        }

        public string Serialize(params DataPoint[] dataPoint)
        {
            return string.Join(Constants.InterDelimiter, dataPoint.Select(r => this.SerializeInternal(r)).ToArray());
        }
        #endregion

        #region privates
        private DataPoint[] ParseFixed(string raw)
        {
            var fixedSize = KeyLength + ValueLength + 1;
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

            var stringTs = dataPoint.ts.ToString(KeyFormat);
            var stringValue = dataPoint.value.ToString(ValueFormat);

            return stringTs + Constants.IntraDelimiter + stringValue;
        }
        #endregion

    }
}
