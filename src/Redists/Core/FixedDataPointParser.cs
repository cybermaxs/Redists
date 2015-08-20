using System;
using System.Linq;
using System.Text;

namespace Redists.Core
{
    internal class FixedDataPointParser : IDataPointParser
    {
        public const int KeyLength = 13;
        public const string KeyFormat = "D13";
        public const int ValueLength = 19;
        public const string ValueFormat = "D19";

        #region publics
        public DataPoint[] Deserialize(string raw)
        {
            var fixedSize = KeyLength + ValueLength + 1;
            var nbItems = raw.Length / (fixedSize + 1);
            var results = new DataPoint[nbItems == 0 ? 1 : nbItems];

            var current = 0;
            var buffer = string.Empty;
            long ts;
            long value;

            while (current < raw.Length)
            {
                buffer = raw.Substring(current, fixedSize);
                long.TryParse(buffer.Substring(0, KeyLength), out ts);
                long.TryParse(buffer.Substring(KeyLength + 1, ValueLength), out value);

                var point = new DataPoint(ts, value);

                results[current / (fixedSize + 1)] = point;
                current += (fixedSize + 1);
            }
            return results;
        }

        public string Serialize(DataPoint dataPoint)
        {
            if (dataPoint == DataPoint.Empty)
                return string.Empty;

            var stringTs = dataPoint.ts.ToString(KeyFormat);
            var stringValue = dataPoint.value.ToString(ValueFormat);

            return stringTs + Constants.IntraDelimiterChar + stringValue;
        }

        public string Serialize(DataPoint[] dataPoints)
        {
            if (dataPoints == null || dataPoints.Length == 0)
                return string.Empty;

            var builder = new StringBuilder();
            foreach (var dp in dataPoints)
            {
                builder.Append(dp.ts.ToString(KeyFormat));
                builder.Append(Constants.IntraDelimiterChar);
                builder.Append(dp.value.ToString(ValueFormat));
                builder.Append(Constants.InterDelimiterChar);
            }

            return builder.ToString();
        }
        #endregion

        #region privates
        private DataPoint[] ParseFixed(string raw)
        {
            var fixedSize = KeyLength + ValueLength + 1;
            var nbItems = raw.Length / (fixedSize + 1);
            var results = new DataPoint[nbItems];

            var current = 0;
            //while (current != raw.Length)
            //{
            //    var dataPoint = this.Deserialize(raw.Substring(current, fixedSize));
            //    results[current / (fixedSize + 1)] = dataPoint;
            //    current += (fixedSize + 1);
            //}
            return results;
        }
        #endregion

    }
}
