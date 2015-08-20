using Redists.Core;
using Xunit;

namespace Redists.Test.Core
{
    public class DynamicDataPointParserTests
    {
        DynamicDataPointParser parser = new DynamicDataPointParser();

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

        [Theory]
        [InlineData("123:456", 123, 456)]
        [InlineData("123:456#", 123, 456)]
        public void Deserialization(string raw, long ts, long value)
        {
            var dataPoint = parser.Deserialize(raw);

            Assert.NotNull(dataPoint);
            Assert.True(dataPoint.Length==1);
            Assert.Equal(ts, dataPoint[0].ts);
            Assert.Equal(value, dataPoint[0].value);
        }

        [Fact]
        public void ParseRaw()
        {
            var dataPoints = parser.Deserialize("111:222#333:444#555:666#");

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
