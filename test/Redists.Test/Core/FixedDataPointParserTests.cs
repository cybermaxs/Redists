using Redists.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Redists.Test.Core
{
    public class FixedDataPointParserTests
    {
        FixedDataPointParser parser = new FixedDataPointParser();

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

            Assert.Equal(33, res.Length);
            Assert.Equal("0000000000123:0000000000000000456", res);
        }

        [Theory]
        [InlineData("0000000000333:0000000000000000444", 333, 444)]
        public void Deserialization(string raw, long ts, long value)
        {

            var dataPoint = parser.Deserialize(raw);

            Assert.NotNull(dataPoint);
            Assert.True(dataPoint.Length == 1);
            Assert.Equal(ts, dataPoint[0].ts);
            Assert.Equal(value, dataPoint[0].value);
        }

        [Fact]
        public void ParseRaw()
        {
            var dataPoints = parser.Deserialize("0000000000111:0000000000000000222#0000000000333:0000000000000000444#0000000000555:0000000000000000666#");

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
