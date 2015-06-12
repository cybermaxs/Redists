using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Redists.Extensions;

namespace Redists.Test.Extensions
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
    }
}
