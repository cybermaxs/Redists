using Moq;
using Ploeh.AutoFixture;
using Redists.Configuration;
using Redists.Core;
using System;
using Xunit;
using Redists.Extensions;

namespace Redists.Tests
{
    public class TimeSeriesClientTests
    {
        private Mock<ITimeSeriesReader> mockOfReader;
        private Mock<ITimeSeriesWriter> mockOfWriter;
        private ITimeSeriesClient client;
        private TimeSeriesOptions options;
        private Fixture fixture = new Fixture();
        private string name = "testclient";


        public TimeSeriesClientTests()
        {
            mockOfReader = new Mock<ITimeSeriesReader>();
            mockOfWriter = new Mock<ITimeSeriesWriter>();

            options = new TimeSeriesOptions(5000, 1000, TimeSpan.MaxValue);
            client = new TimeSeriesClient(name, options, mockOfReader.Object, mockOfWriter.Object);
        }

        [Fact]
        public void AppendSingleAsync()
        {
            var written = this.fixture.Create<long>();
            mockOfWriter.Setup(w => w.AppendAsync(It.IsAny<string>(), It.IsAny<DataPoint[]>())).ReturnsAsync(written);

            var at = fixture.Create<DateTime>();
            var val = fixture.Create<long>();

            var t = client.AddAsync(at, val);

            Assert.Equal(written, t.Result);
            mockOfWriter.Verify(w => w.AppendAsync("ts#testclient#" + at.ToRoundedTimestamp(options.KeyNormFactor).ToString(), new DataPoint[] { new DataPoint(at.ToRoundedTimestamp(options.DataPointNormFactor), val) }), Times.Once);
        }

        [Fact]
        public void AppendMultipleAsync_WhenEmpty_ShouldReturn0()
        {
            var res = client.AddAsync();
            Assert.Equal(0L, res.Result);

            res = client.AddAsync(new DataPoint[] { });
            Assert.Equal(0L, res.Result);
        }
        [Fact]
        public void AppendMultipleAsync()
        {
            var written = this.fixture.Create<long>();
            mockOfWriter.Setup(w => w.AppendAsync(It.IsAny<string>(), It.IsAny<DataPoint[]>())).ReturnsAsync(written);

            var at = fixture.Create<DateTime>();
            var at2 = at.AddMilliseconds(options.KeyNormFactor);
            var val = fixture.Create<long>();

            var t = client.AddAsync(new DataPoint(at, val), new DataPoint(at.AddSeconds(5), val));

            Assert.Equal(2 * written, t.Result);
            mockOfWriter.Verify(w => w.AppendAsync("ts#testclient#" + at.ToRoundedTimestamp(options.KeyNormFactor).ToString(), new DataPoint[] { new DataPoint(at.ToRoundedTimestamp(options.DataPointNormFactor), val) }), Times.Once);
            mockOfWriter.Verify(w => w.AppendAsync("ts#testclient#" + at2.ToRoundedTimestamp(options.KeyNormFactor).ToString(), new DataPoint[] { new DataPoint(at2.ToRoundedTimestamp(options.DataPointNormFactor), val) }), Times.Once);
        }

        [Fact]
        public void AllSinceAsync_NoUtc_ShouldThrowEx()
        {
            Assert.Throws<ArgumentException>(() => { client.AllSinceAsync(DateTime.Now); });
        }

        [Fact]
        public void RangeAsync_NoUtc_ShouldThrowEx()
        {
            Assert.ThrowsAsync<ArgumentException>(() => { return client.RangeAsync(DateTime.Now, DateTime.UtcNow); });
            Assert.ThrowsAsync<ArgumentException>(() => { return client.RangeAsync(DateTime.UtcNow, DateTime.Now); });
        }

        [Fact]
        public void RangeAsync_ShouldPass()
        {
            mockOfReader.Setup(r => r.ReadAllAsync(It.IsAny<string>())).ReturnsAsync(new DataPoint[] { new DataPoint(DateTime.UtcNow, 12345678) });

            var to = DateTime.UtcNow;
            var from = to.AddMilliseconds(-options.KeyNormFactor);

            var t = client.RangeAsync(from, to);

            Assert.Equal(2, t.Result.Length);
            mockOfReader.Verify(r => r.ReadAllAsync("ts#testclient#" + from.ToRoundedTimestamp(options.KeyNormFactor).ToString()), Times.Once);
            mockOfReader.Verify(r => r.ReadAllAsync("ts#testclient#" + to.ToRoundedTimestamp(options.KeyNormFactor).ToString()), Times.Once);

        }

    }
}
