using Moq;
using StackExchange.Redis;
using System;
using Xunit;

namespace Redists.Test
{
    public class TimeSeriesFactoryTests
    {
        [Fact]
        public void Ctor_BadArgs_ShouldThrowException()
        {
            // dbAsync
            Assert.Throws<ArgumentNullException>(() => { var ts = TimeSeriesFactory.New(null, "foobar", new Redists.Configuration.TimeSeriesOptions(2, 1, false, null)); });

            Mock<IDatabaseAsync> mockOFDb = new Mock<IDatabaseAsync>();
            
            // name
            Assert.Throws<ArgumentNullException>(() => { var ts = TimeSeriesFactory.New(mockOFDb.Object, string.Empty, new Redists.Configuration.TimeSeriesOptions(2, 1, false, null)); });
            // name
            Assert.Throws<ArgumentNullException>(() => { var ts = TimeSeriesFactory.New(mockOFDb.Object, "foobar", null); });
        }
    }
}
