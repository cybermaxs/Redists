using System;
using System.Linq;
using Xunit;
using Redists.Extensions;

namespace Redists.Tests.Extensions
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void Epoch()
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0);
            Assert.Equal(0, epoch.ToTimestamp());
            Assert.Equal(0, epoch.ToSecondsTimestamp());
        }

        [Fact]
        public void AnyDateShouldBeValid()
        {
            var now = new DateTime(2015, 6, 10, 22, 3, 15);
            Assert.Equal(1433973795000, now.ToTimestamp());
            Assert.Equal(1433973795, now.ToSecondsTimestamp());

            Assert.Equal(1433973780000, now.ToRoundedTimestamp(60*1000));
            Assert.Equal(1433973600000, now.ToRoundedTimestamp(3600 * 1000));

            Assert.Equal(1433973780, now.ToRoundedSecondsTimestamp(60));
            Assert.Equal(1433973600, now.ToRoundedSecondsTimestamp(3600));
        }

        [Fact]
        public void ToKeyDateTimes_ShouldBeRight()
        {
            var from = new DateTime(2015, 6, 10, 18, 3, 15);
            var to = new DateTime(2015, 6, 10, 22, 3, 15);

            var res = from.ToKeyDateTimes(to, 3600*1000);

            Assert.Equal(5, res.Count());
            Assert.Equal(new DateTime(2015, 6, 10, 18, 0, 0), res[0]);
            Assert.Equal(new DateTime(2015, 6, 10, 19, 0, 0), res[1]);
            Assert.Equal(new DateTime(2015, 6, 10, 20, 0, 0), res[2]);
            Assert.Equal(new DateTime(2015, 6, 10, 21, 0, 0), res[3]);
            Assert.Equal(new DateTime(2015, 6, 10, 22, 0, 0), res[4]);
        }

        [Fact]
        public void ToKeyDateTimes_WhenEqual_ShouldBeSame()
        {
            var from = new DateTime(2015, 6, 10, 18, 3, 15);
            var to = new DateTime(2015, 6, 10, 18, 8, 15);

            var res = from.ToKeyDateTimes(to, 3600*1000);

            Assert.Equal(1, res.Count());
            Assert.Equal(new DateTime(2015, 6, 10, 18, 0, 0), res[0]);
        }
    }
}
