using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Redists.Test
{

    public class RecordTests
    {
        [Fact]
        public void EqualityTests_WhenEqual()
        {
            var r1 = new Record(123, 456);
            var r2 = new Record(123, 456);

            Assert.True(r1 == r2);
            Assert.True(r1.Equals(r2));
            Assert.Equal(r1, r2);
            Assert.False(r1 != r2);
        }

        [Fact]
        public void EqualityTests_WhenNotEqual()
        {
            var r1 = new Record(123, 456);
            var r2 = new Record(456, 789);

            Assert.False(r1 == r2);
            Assert.False(r1.Equals(r2));
            Assert.NotEqual(r1, r2);
            Assert.True(r1 != r2);
        }
    }
}
