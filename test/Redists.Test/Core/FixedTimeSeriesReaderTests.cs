using Moq;
using Ploeh.AutoFixture;
using Redists.Core;
using StackExchange.Redis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Redists.Test.Core
{
    public class FixedTimeSeriesReaderTests
    {
        private readonly TimeSeriesReader reader;
        private Fixture fixture = new Fixture();
        private Mock<IDatabase> mockOfDb;

        public FixedTimeSeriesReaderTests()
        {
            mockOfDb = new Mock<IDatabase>();
            mockOfDb.Setup(db => db.StringGetRangeAsync("short", It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CommandFlags>())).ReturnsAsync(Generate(100));
            mockOfDb.Setup(db => db.StringGetRangeAsync("long", It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CommandFlags>())).Returns<RedisKey, long, long, CommandFlags>(this.GeneratePartial5000);
            mockOfDb.Setup(db => db.StringGetRangeAsync("broken", It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CommandFlags>())).Returns<RedisKey, long, long, CommandFlags>(this.GeneratePartial5000);
            var parser = new FixedDataPointParser();
            reader = new TimeSeriesReader(mockOfDb.Object, parser);
        }

        [Fact]
        public void Read_WheShort_ShouldPass()
        {
            var t = reader.ReadAllAsync("short");

            Assert.NotNull(t);
            t.Wait();
            Assert.False(t.IsFaulted);
            Assert.False(t.IsCanceled);
            var dataPoints = t.Result;
            Assert.Equal(100, dataPoints.Length);

            mockOfDb.Verify(db => db.StringGetRangeAsync("short", It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CommandFlags>()), Times.Once);
        }

        [Fact]
        public void Read_WhenLong_ShouldPass()
        {
            var t = reader.ReadAllAsync("long");

            Assert.NotNull(t);
            t.Wait();
            Assert.False(t.IsFaulted);
            Assert.False(t.IsCanceled);
            var dataPoints = t.Result;
            Assert.Equal(5000, dataPoints.Length);

            mockOfDb.Verify(db => db.StringGetRangeAsync("long", It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CommandFlags>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Read_WhenBroken_ShouldPass()
        {
            var t = reader.ReadAllAsync("broken");

            Assert.NotNull(t);
            t.Wait();
            Assert.False(t.IsFaulted);
            Assert.False(t.IsCanceled);
            var dataPoints = t.Result;
            Assert.Equal(5000, dataPoints.Length);

            mockOfDb.Verify(db => db.StringGetRangeAsync("broken", It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CommandFlags>()), Times.AtLeastOnce);
        }

        #region Privates
        private string Generate(int nbItems=100)
        {
            FixedDataPointParser parser = new FixedDataPointParser();
            StringBuilder builder = new StringBuilder();
            foreach (var i in Enumerable.Range(1, nbItems))
            {
                builder.Append(parser.Serialize(new DataPoint(10000+i,i))+Constants.InterDelimiter);
            }
            return builder.ToString();
        }

        private Task<RedisValue> GeneratePartial5000(RedisKey k, long start, long end, CommandFlags _)
        {
            FixedDataPointParser parser = new FixedDataPointParser();
            //generate data
            StringBuilder builder = new StringBuilder();
            foreach (var i in Enumerable.Range(1, 5000))
            {
                builder.Append(parser.Serialize(new DataPoint(10000 + i, i)) + Constants.InterDelimiter);
            }
            var all = builder.ToString();

            if (start == all.Length)
                return Task.FromResult<RedisValue>(string.Empty);

            var to = end > all.Length ? (int)(all.Length - start) : (int)(end - start);

            var partial = all.Substring((int)start, to);
            if (partial.Length != (end - start))
                partial += "dflksmdfk";

            return Task.FromResult((RedisValue)partial);
        }
        #endregion
    }
}
