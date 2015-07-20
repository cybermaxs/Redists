using System;
using System.Linq;

namespace Redists.Core
{
    internal class DynamicRecordParser : IRecordParser
    {
        #region publics
        public Record[] ParseRawString(string raw)
        {
            return ParseDynamic(raw);
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

            return stringTs + Constants.IntraRecordDelimiter + stringValue;
        }
        #endregion

        #region privates
        private Record[] ParseDynamic(string raw)
        {
            var parts = raw.Split(new char[] { Constants.InterRecordDelimiter }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Select(p => Deserialize(p)).ToArray();
        }
        #endregion

    }
}
