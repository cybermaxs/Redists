using System;
using System.Linq;

namespace Redists.Core
{
    internal class DynamicDataPointParser : IDataPointParser
    {
        #region publics
        public DataPoint[] ParseRawString(string raw)
        {
            return ParseDynamic(raw);
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
            return string.Join(Constants.InterDelimiter.ToString(), dataPoint.Select(r => this.SerializeInternal(r)).ToArray());
        }

        #endregion

        #region privates
        private DataPoint[] ParseDynamic(string raw)
        {
            var parts = raw.Split(new char[] { Constants.InterDelimiter }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Select(p => Deserialize(p)).ToArray();
        }

        private string SerializeInternal(DataPoint dataPoint)
        {
            if (dataPoint == DataPoint.Empty)
                return string.Empty;

            var stringTs = dataPoint.ts.ToString();
            var stringValue = dataPoint.value.ToString();

            return stringTs + Constants.IntraDelimiter + stringValue;
        }
        #endregion

    }
}
