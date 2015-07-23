using System;
using System.Linq;
using Redists.Extensions;
using System.Collections.Generic;

namespace Redists.Core
{
    internal class FixedRecordParser : IRecordParser
    {
        public static int FixedKeyLength = 13;
        public static int FixedValueLength = 19;

        static FixedRecordParser()
        {
            FixedKeyLength = DateTime.UtcNow.ToTimestamp().ToString().Length;
            FixedValueLength = long.MaxValue.ToString().Length;
        }

        #region publics
        public Record[] ParseRawString(string raw)
        {
            return ParseFixed(raw);
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

        public string Serialize(params Record[] records)
        {
            if (records == null || records.Count() == 0)
                return string.Empty;

            return string.Join(Constants.InterRecordDelimiter.ToString(), records.Select(r=>this.SerializeInternal(r)).ToArray());
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

        private string SerializeInternal(Record record)
        {
            if (record == Record.Empty)
                return string.Empty;

            var stringTs = record.ts.ToString().PadLeft(FixedKeyLength, Constants.RecordPadChar);
            var stringValue = record.value.ToString().PadLeft(FixedValueLength, Constants.RecordPadChar);

            return stringTs + Constants.IntraRecordDelimiter + stringValue;
        }
        #endregion

    }
}
