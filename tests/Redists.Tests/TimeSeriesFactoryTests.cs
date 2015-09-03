using Moq;
using Redists.Configuration;
using StackExchange.Redis;
using System;
using Xunit;

namespace Redists.Tests
{
    public class TimeSeriesFactoryTests
    {
        Mock<IDatabaseAsync> mockOfDb;
        Mock<IConnectionMultiplexer> mockOfMux;

        public TimeSeriesFactoryTests()
        {
            mockOfDb = new Mock<IDatabaseAsync>();
            mockOfMux = new Mock<IConnectionMultiplexer>();
            
        }
        #region Dynamic
        [Fact]
        public void CtorNewDynamic_BadArgs_ShouldThrowException()
        {
            // dbAsync
            Assert.Throws<ArgumentNullException>(() => { var ts = TimeSeriesFactory.New(null, "foobar", new TimeSeriesOptions(2, 1, null)); });          
            // name
            Assert.Throws<ArgumentNullException>(() => { var ts = TimeSeriesFactory.New(mockOfDb.Object, string.Empty, new TimeSeriesOptions(2, 1, null)); });
            // options
            Assert.Throws<ArgumentNullException>(() => { var ts = TimeSeriesFactory.New(mockOfDb.Object, "foobar", null); });
            // mux status
            Assert.Throws<InvalidOperationException>(() => { var ts = TimeSeriesFactory.New(mockOfDb.Object, "foobar", new TimeSeriesOptions(1, 1, null)); });
        }
        #endregion

        [Fact]
        public void CtorNewFixed_BadArgs_ShouldThrowException()
        {
            // dbAsync
            Assert.Throws<ArgumentNullException>(() => { var ts = TimeSeriesFactory.NewFixed(null, "foobar", new TimeSeriesOptions(2, 1, null)); });
            // name
            Assert.Throws<ArgumentNullException>(() => { var ts = TimeSeriesFactory.NewFixed(mockOfDb.Object, string.Empty, new TimeSeriesOptions(2, 1, null)); });
            // name
            Assert.Throws<ArgumentNullException>(() => { var ts = TimeSeriesFactory.NewFixed(mockOfDb.Object, "foobar", null); });
            // mux status
            Assert.Throws<InvalidOperationException>(() => { var ts = TimeSeriesFactory.NewFixed(mockOfDb.Object, "foobar", new TimeSeriesOptions(1, 1, null)); });
        }
    }
}
