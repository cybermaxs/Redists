using Redists.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Redists.Test
{
    public class TimeSeriesClientTests
    {
        [Fact]
        public void AddAsync()
        {
            
            //TimeSeriesClient client = new TimeSeriesClient("foobar", new TimeSeriesOptions(20, 10, false, null), );
            //DataPoint p = new DataPoint(123, 456);
            Assert.Equal("0003", 3.ToString("D4"));
           
        }
    }
}
