using Moq;
using Ploeh.AutoFixture;
using Redists.Core;
using StackExchange.Redis;
using System;
using Xunit;

namespace Redists.Test.Core
{
    public class DynamicTimeSeriesWriterTests
    {
        private readonly TimeSeriesWriter writer;
        private Fixture fixture = new Fixture();
        private Mock<IDatabase> mockOfDb;

        public DynamicTimeSeriesWriterTests()
        {
            mockOfDb = new Mock<IDatabase>();
            mockOfDb.Setup(db => db.StringAppendAsync(It.IsAny<string>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>())).ReturnsAsync(10);
            var parser = new DynamicDataPointParser(); ;
            writer = new TimeSeriesWriter(mockOfDb.Object, parser, TimeSpan.MaxValue);
        }

        [Fact]
        public void WriteShouldPass()
        {
            var key = this.fixture.Create<string>();
            var dataPoint = new DataPoint(123, 456);
            var t = writer.AppendAsync(key, new DataPoint[] { dataPoint });

            Assert.NotNull(t);
            t.Wait();
            Assert.False(t.IsFaulted);
            Assert.False(t.IsCanceled);

            mockOfDb.Verify(db => db.StringAppendAsync(key, "123:456#", It.IsAny<CommandFlags>()), Times.Once);
        }
    }
}
