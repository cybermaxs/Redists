using System;
using System.Linq;
using Redists.Extensions;

namespace Redists.Core
{
    internal class RecordParser : IRecordParser
    {
        public static int FixedKeyLength = 13;
        public static int FixedValueLength = 19;

        static RecordParser()
        {
            FixedKeyLength = DateTime.UtcNow.ToTimestamp().ToString().Length;
            FixedValueLength = long.MaxValue.ToString().Length;
        }

        private readonly bool isFixed;
        public RecordParser(bool isFixed)
        {
            this.isFixed = isFixed;
        }

        #region publics
        public Record[] ParseRawString(string raw)
        {
            return this.isFixed ? ParseFixed(raw) : ParseDynamic(raw);
        }
        public Record Deserialize(string rawRecord)
        {
            var parts = rawRecord.Split(Constants.IntraRecordDelimiter);
            long ts;
            long value;
            long.TryParse(parts[0], out ts);
            long.TryParse(parts[1], out value);

            return new Record(ts, value);
        }

        public string Serialize(Record record)
        {
            if (record == Record.Empty)
                return string.Empty;

            var stringTs = record.ts.ToString();
            var stringValue = record.value.ToString();

            if (this.isFixed)
            {
                stringTs = stringTs.PadLeft(FixedKeyLength, Constants.RecordPadChar);
                stringValue = stringValue.PadLeft(FixedValueLength, Constants.RecordPadChar);
            }

            return stringTs + Constants.IntraRecordDelimiter + stringValue;
        }
        #endregion

        #region privates
        private Record[] ParseFixed(string raw)
        {
            var recordFixedSize = FixedKeyLength + FixedValueLength + 1;
            var nbItems = raw.Length / (recordFixedSize + 1);
            var results = new Record[nbItems];

            int current = 0;
            while (current != raw.Length)
            {
                Record record = this.Deserialize(raw.Substring(current, recordFixedSize));
                results[current / (recordFixedSize + 1)] = record;
                current += (recordFixedSize + 1);
            }
            return results;
        }

        private Record[] ParseDynamic(string raw)
        {
            var parts = raw.Split(new char[] { Constants.InterRecordDelimiter }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Select(p => Deserialize(p)).ToArray();
        }
        #endregion

    }
}
