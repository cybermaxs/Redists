using Moq;
using Ploeh.AutoFixture;
using Redists.Core;
using StackExchange.Redis;
using System;
using Xunit;

namespace Redists.Tests.Core
{
    public class FixedTimeSeriesWriterTests
    {
        private readonly TimeSeriesWriter writerWithExpiration;
        private readonly TimeSeriesWriter writerWithoutExpiration;
        private Fixture fixture = new Fixture();
        private Mock<IDatabase> mockOfDb;

        public FixedTimeSeriesWriterTests()
        {
            mockOfDb = new Mock<IDatabase>();
            mockOfDb.Setup(db => db.StringAppendAsync(It.IsAny<string>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>())).ReturnsAsync(10);
            var parser = new FixedDataPointParser();

            writerWithExpiration = new TimeSeriesWriter(mockOfDb.Object, parser, TimeSpan.FromDays(1));
            writerWithoutExpiration = new TimeSeriesWriter(mockOfDb.Object, parser, null);
        }

        [Fact]
        public void WhenAppend_ShouldCallStringAppendAndKeyExpire()
        {
            var key = this.fixture.Create<string>();
            var dataPoint = new DataPoint(123, 456);
            var t = writerWithExpiration.AppendAsync(key, new DataPoint[] { dataPoint });

            Assert.NotNull(t);
            t.Wait();
            Assert.False(t.IsFaulted);
            Assert.False(t.IsCanceled);

            mockOfDb.Verify(db => db.StringAppendAsync(key, "0000000000123:0000000000000000456#", It.IsAny<CommandFlags>()), Times.Once);
            mockOfDb.Verify(db => db.KeyExpireAsync(key, TimeSpan.FromDays(1), It.IsAny<CommandFlags>()), Times.Once);
        }

        [Fact]
        public void WhenAppend_ShouldCallStringAppendOnly()
        {
            var key = this.fixture.Create<string>();
            var dataPoint = new DataPoint(123, 456);
            var t = writerWithoutExpiration.AppendAsync(key, new DataPoint[] { dataPoint });

            Assert.NotNull(t);
            t.Wait();
            Assert.False(t.IsFaulted);
            Assert.False(t.IsCanceled);

            mockOfDb.Verify(db => db.StringAppendAsync(key, "0000000000123:0000000000000000456#", It.IsAny<CommandFlags>()), Times.Once);
            mockOfDb.Verify(db => db.KeyExpireAsync(key, TimeSpan.FromDays(1), It.IsAny<CommandFlags>()), Times.Never);
        }
    }
}
