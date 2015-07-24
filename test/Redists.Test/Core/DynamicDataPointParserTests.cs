using Redists.Core;
using Xunit;

namespace Redists.Test.Core
{
    public class DynamicDataPointParserTests
    {
        IDataPointParser parser = new DynamicDataPointParser();

        [Fact]
        public void SerializationWithDefault()
        {
            var res = parser.Serialize(DataPoint.Empty);

            Assert.Equal(0, res.Length);
            Assert.Equal(string.Empty, res);
        }

        [Fact]
        public void Serialization()
        {
            var dataPoint = new DataPoint(123, 456);
            var res = parser.Serialize(dataPoint);

            Assert.Equal(7, res.Length);
            Assert.Equal("123:456", res);
        }

        [Fact]
        public void Deserialization()
        {
            var dataPoint = parser.Deserialize("333:444");

            Assert.NotNull(dataPoint);
            Assert.Equal(333, dataPoint.ts);
            Assert.Equal(444,dataPoint.value);
        }

        [Fact]
        public void ParseRaw()
        {
            var dataPoints = parser.ParseRawString("111:222#333:444#555:666#");

            Assert.NotNull(dataPoints);
            Assert.Equal(3, dataPoints.Length);
            Assert.Equal(111, dataPoints[0].ts);
            Assert.Equal(222, dataPoints[0].value);
            Assert.Equal(333, dataPoints[1].ts);
            Assert.Equal(444, dataPoints[1].value);
            Assert.Equal(555, dataPoints[2].ts);
            Assert.Equal(666, dataPoints[2].value);
        }
    }
}
