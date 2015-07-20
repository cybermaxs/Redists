using Redists.Core;
using Xunit;

namespace Redists.Test.Core
{
    public class DynamicRecordParserTests
    {
        IRecordParser parser = new DynamicRecordParser();

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

            Assert.Equal(7, res.Length);
            Assert.Equal("123:456", res);
        }

        [Fact]
        public void Deserialization()
        {
            var record = parser.Deserialize("333:444");

            Assert.NotNull(record);
            Assert.Equal(333, record.ts);
            Assert.Equal(444,record.value);
        }

        [Fact]
        public void ParseRaw()
        {
            var records = parser.ParseRawString("111:222#333:444#555:666#");

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
