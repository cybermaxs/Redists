using System.Collections.Generic;
using System.Text;

namespace Redists.Core
{
    internal sealed class DynamicDataPointParser : IStringParser<DataPoint>
    {
        public DataPoint[] Parse(string raw)
        {
            if (string.IsNullOrEmpty(raw))
                return new DataPoint[0];

            var points = new List<DataPoint>();
            var startIndex = 0;
            var currentIndex = 0;

            var buffer = string.Empty;
            long ts;
            long value;

            while (startIndex < raw.Length)
            {
                currentIndex = raw.IndexOf(Constants.InterDelimiterChar, startIndex);

                if (currentIndex == -1) //last part
                    currentIndex = raw.Length;

                if (currentIndex != startIndex)//delimiter at first char
                {
                    buffer = raw.Substring(startIndex, currentIndex - startIndex);
                    var intraIndex = buffer.IndexOf(Constants.IntraDelimiterChar);
                    long.TryParse(buffer.Substring(0, intraIndex), out ts);
                    long.TryParse(buffer.Substring(intraIndex + 1, buffer.Length - intraIndex - 1), out value);

                    var point = new DataPoint(ts, value);
                    points.Add(point);
                }

                startIndex = currentIndex + 1;
            }

            return points.ToArray();
        }

        public string Serialize(DataPoint[] dataPoints)
        {
            if (dataPoints == null || dataPoints.Length == 0)
                return string.Empty;

            var fixedSize = 13 + 2 + 4; //ts + delimiters + value of 1000
            var builder = new StringBuilder(dataPoints.Length * fixedSize);
            foreach (var dp in dataPoints)
            {
                builder.Append(dp.ts);
                builder.Append(Constants.IntraDelimiterChar);
                builder.Append(dp.value);
                builder.Append(Constants.InterDelimiterChar);
            }

            return builder.ToString();
        }
    }
}
