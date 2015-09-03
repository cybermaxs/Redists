using Redists.Configuration;
using System;
using Xunit;

namespace Redists.Tests.Configuration
{
    public class TimeSeriesOptionsTests
    {
        [Fact]
        public void Ctor_BadArgs_ShouldThrowException()
        {
            // key factor vs dataPoint factor 
            Assert.Throws<InvalidOperationException>(() => { TimeSeriesOptions options = new TimeSeriesOptions(100, 200, TimeSpan.MaxValue); });

            // ttl value vs key factor 
            Assert.Throws<InvalidOperationException>(() => { TimeSeriesOptions options = new TimeSeriesOptions(200, 100, TimeSpan.MinValue); });
        }

        [Fact]
        public void Ctor_ShouldPass()
        {
            var options = new TimeSeriesOptions(200, 100, TimeSpan.MaxValue);

            Assert.Equal(200, options.KeyNormFactor);
            Assert.Equal(100, options.DataPointNormFactor);
            Assert.Equal(TimeSpan.MaxValue, options.KeyTtl);
        }
    }
}
