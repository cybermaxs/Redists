using Moq;
using Ploeh.AutoFixture;
using Redists.Configuration;
using Redists.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            mockOfWriter.Verify(w => w.AppendAsync("ts#testclient#"+ at.ToRoundedTimestamp(options.KeyNormFactor).ToString(), new DataPoint[] { new DataPoint(at.ToRoundedTimestamp(options.DataPointNormFactor), val) }), Times.Once);
        }
    }
}
