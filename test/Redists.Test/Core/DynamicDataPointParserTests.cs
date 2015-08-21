using Redists.Core;
using Xunit;

namespace Redists.Test.Core
{
    public class DynamicDataPointParserTests
    {
        DynamicDataPointParser parser = new DynamicDataPointParser();

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
        [InlineData(123, 456, "123:456#")]
        public void Serialize_Default(long ts, long value, string expected)
        {
            var dataPoint = new DataPoint(ts, value);
            var res = parser.Serialize(new DataPoint[] { dataPoint });

            Assert.Equal(expected.Length, res.Length);
            Assert.Equal(expected, res);
        }

        [Theory]
        [InlineData("123:456", 123, 456)]
        [InlineData("123:456#", 123, 456)]
        [InlineData("#123:456#", 123, 456)]
        [InlineData("##123:456#", 123, 456)]
        public void Parse_WhenSingle_ShouldPass(string raw, long ts, long value)
        {
            var dataPoint = parser.Parse(raw);

            Assert.NotNull(dataPoint);
            Assert.True(dataPoint.Length == 1);
            Assert.Equal(ts, dataPoint[0].ts);
            Assert.Equal(value, dataPoint[0].value);
        }

        [Fact]
        public void Parse_WhenMultiple_ShouldPass()
        {
            var dataPoints = parser.Parse("111:222#333:444#555:666#");

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
