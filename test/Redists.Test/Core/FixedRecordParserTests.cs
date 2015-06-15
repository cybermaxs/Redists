using Redists.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Redists.Test.Core
{
    public class FixedRecordParserTests
    {
        RecordParser parser = new RecordParser(true);

        [Fact]
        public void SerializationWithDefault()
        {
            var res = parser.Serialize(Record.Empty);

            Assert.Equal(0, res.Length);
            Assert.Equal(string.Empty, res);
        }

        [Fact]
        public void Serialization()
        {
            var record = new Record(123, 456);
            var res = parser.Serialize(record);

            Assert.Equal(33, res.Length);
            Assert.Equal("0000000000123:0000000000000000456", res);
        }

        [Fact]
        public void Deserialization()
        {
            var record = parser.Deserialize("0000000000333:0000000000000000444");

            Assert.NotNull(record);
            Assert.Equal(333, record.ts);
            Assert.Equal(444, record.value);
        }

        [Fact]
        public void ParseRaw()
        {
            var records = parser.ParseRawString("0000000000111:0000000000000000222#0000000000333:0000000000000000444#0000000000555:0000000000000000666#");

            Assert.NotNull(records);
            Assert.Equal(3, records.Length);
            Assert.Equal(111, records[0].ts);
            Assert.Equal(222, records[0].value);
            Assert.Equal(333, records[1].ts);
            Assert.Equal(444, records[1].value);
            Assert.Equal(555, records[2].ts);
            Assert.Equal(666, records[2].value);
        }
    }
}
