using System.Text;

namespace Redists.Core
{
    internal sealed class FixedDataPointParser : IStringParser<DataPoint>
    {

        public const int KeyLength = 13;
        public const string KeyFormat = "D13";
        public const int ValueLength = 19;
        public const string ValueFormat = "D19";

        private readonly char intraDelimiter;
        private readonly char interDelimiter;
        public FixedDataPointParser(char interDelimiter, char intraDelimiter)
        {
            this.interDelimiter = interDelimiter;
            this.intraDelimiter = intraDelimiter;
        }

        public DataPoint[] Parse(string raw)
        {
            const int fixedSize = KeyLength + ValueLength + 1;
            var nbItems = raw.Length / (fixedSize + 1);
            var results = new DataPoint[nbItems == 0 ? 1 : nbItems];

            var current = 0;
            var buffer = string.Empty;
            long ts;
            long value;

            while (current < raw.Length)
            {
                if(raw[current]== interDelimiter)
                {
                    current++;
                    continue;
                }
                buffer = raw.Substring(current, fixedSize);
                long.TryParse(buffer.Substring(0, KeyLength), out ts);
                long.TryParse(buffer.Substring(KeyLength + 1, ValueLength), out value);

                var point = new DataPoint(ts, value);

                results[current / (fixedSize + 1)] = point;
                current += (fixedSize + 1);
            }
            return results;
        }

        public string Serialize(DataPoint[] dataPoints)
        {
            if (dataPoints == null || dataPoints.Length == 0)
                return string.Empty;

            const int fixedSize = KeyLength + ValueLength + 2;

            var builder = new StringBuilder(dataPoints.Length * fixedSize);
            foreach (var dp in dataPoints)
            {
                builder.Append(dp.timestamp.ToString(KeyFormat));
                builder.Append(intraDelimiter);
                builder.Append(dp.value.ToString(ValueFormat));
                builder.Append(interDelimiter);
            }

            return builder.ToString();
        }
    }
}
