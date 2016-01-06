using Redists.Core;
using Xunit;

namespace Redists.Tests.Core
{
    public class FixedDataPointParserTests
    {
        FixedDataPointParser parser = new FixedDataPointParser(Constants.DefaultInterDelimiterChar, Constants.DefaultIntraDelimiterChar);

        [Fact]
        public void Serialize_EdgesCases_ShouldPass()
        {
            //null
            var res1 = parser.Serialize(null);
            Assert.Equal(0, res1.Length);
            Assert.Equal(string.Empty, res1);

            //empty array
            var res2 = parser.Serialize(new DataPoint[0]);
            Assert.Equal(0, res2.Length);
            Assert.Equal(string.Empty, res2);
        }

        [Theory]
        [InlineData(123, 456, "0000000000123:0000000000000000456#")]
        public void Serialize_Default(long ts, long value, string expected)
        {
            var dataPoint = new DataPoint(ts, value);
            var res = parser.Serialize(new DataPoint[] { dataPoint });

            Assert.Equal(expected.Length, res.Length);
            Assert.Equal(expected, res);
        }

        [Theory]
        [InlineData("0000000000333:0000000000000000444", 333, 444)]
        [InlineData("0000000000333:0000000000000000444#", 333, 444)]
        [InlineData("#0000000000333:0000000000000000444#", 333, 444)]
        [InlineData("##0000000000333:0000000000000000444#", 333, 444)]
        public void Deserialization(string raw, long ts, long value)
        {

            var dataPoint = parser.Parse(raw);

            Assert.NotNull(dataPoint);
            Assert.True(dataPoint.Length == 1);
            Assert.Equal(ts, dataPoint[0].timestamp);
            Assert.Equal(value, dataPoint[0].value);
        }

        [Fact]
        public void Parse_WhenMultiple_ShouldPass()
        {
            var dataPoints = parser.Parse("0000000000111:0000000000000000222#0000000000333:0000000000000000444#0000000000555:0000000000000000666#");

            Assert.NotNull(dataPoints);
            Assert.Equal(3, dataPoints.Length);
            Assert.Equal(111, dataPoints[0].timestamp);
            Assert.Equal(222, dataPoints[0].value);
            Assert.Equal(333, dataPoints[1].timestamp);
            Assert.Equal(444, dataPoints[1].value);
            Assert.Equal(555, dataPoints[2].timestamp);
            Assert.Equal(666, dataPoints[2].value);
        }
    }
}
