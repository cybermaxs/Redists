using Moq;
using Redists.Configuration;
using StackExchange.Redis;
using System;
using Xunit;

namespace Redists.Tests
{
    public class TimeSeriesFactoryTests
    {
        [Fact]
        public void Ctor_BadArgs_ShouldThrowException()
        {
            // dbAsync
            Assert.Throws<ArgumentNullException>(() => { var ts = TimeSeriesFactory.New(null, "foobar", new TimeSeriesOptions(2, 1, null)); });

            Mock<IDatabaseAsync> mockOFDb = new Mock<IDatabaseAsync>();
            
            // name
            Assert.Throws<ArgumentNullException>(() => { var ts = TimeSeriesFactory.New(mockOFDb.Object, string.Empty, new TimeSeriesOptions(2, 1, null)); });
            // name
            Assert.Throws<ArgumentNullException>(() => { var ts = TimeSeriesFactory.New(mockOFDb.Object, "foobar", null); });
        }
    }
}
