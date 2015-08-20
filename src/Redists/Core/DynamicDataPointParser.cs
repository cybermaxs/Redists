using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redists.Core
{
    internal sealed class DynamicDataPointParser : IDataPointParser
    {
        #region publics
        public DataPoint[] Deserialize(string rawString)
        {
            if (string.IsNullOrEmpty(rawString))
                return new DataPoint[0];

            var points = new List<DataPoint>();
            var startIndex = 0;
            var currentIndex = 0;

            var buffer = string.Empty;
            long ts;
            long value;

            while (startIndex < rawString.Length)
            {
                currentIndex = rawString.IndexOf(Constants.InterDelimiterChar, startIndex);

                if (currentIndex == -1)
                    currentIndex = rawString.Length;

                buffer = rawString.Substring(startIndex, currentIndex - startIndex);
                var intraIndex = buffer.IndexOf(Constants.IntraDelimiterChar);
                long.TryParse(buffer.Substring(0, intraIndex), out ts);
                long.TryParse(buffer.Substring(intraIndex + 1, buffer.Length - intraIndex - 1), out value);

                var point = new DataPoint(ts, value);
                points.Add(point);

                startIndex = currentIndex + 1;
            }

            return points.ToArray();
        }

        public string Serialize(DataPoint dataPoint)
        {
            if (dataPoint == DataPoint.Empty)
                return string.Empty;

            var stringTs = dataPoint.ts.ToString();
            var stringValue = dataPoint.value.ToString();

            return string.Concat(stringTs, Constants.IntraDelimiterChar.ToString(), stringValue);
        }

        public string Serialize(DataPoint[] dataPoints)
        {
            if (dataPoints == null || dataPoints.Length == 0)
                return string.Empty;

            var builder = new StringBuilder();
            foreach (var dp in dataPoints)
            {
                builder.Append(dp.ts);
                builder.Append(Constants.IntraDelimiterChar);
                builder.Append(dp.value);
                builder.Append(Constants.InterDelimiterChar);
            }

            return builder.ToString();
        }

        #endregion
    }
}
