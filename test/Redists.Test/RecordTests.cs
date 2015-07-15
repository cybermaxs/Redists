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
        public void ToString_ShouldReturnCorrectValue()
        {
            var r = new Record(123, 456);
            var str = r.ToString();
            Assert.Equal("123:456", str);
        }

        [Fact]
        public void EqualityTests_WhenSame_ShouldBeEqual()
        {
            var r1 = new Record(123, 456);
            var r2 = new Record(123, 456);

            Assert.True(r1 == r2);
            Assert.True(r1.Equals(r2));
            Assert.Equal(r1.GetHashCode(), r2.GetHashCode());
            Assert.Equal(r1, r2);
            Assert.False(r1 != r2);
        }

        [Fact]
        public void EqualityTests_WhenDiff__ShouldNotEqual()
        {
            var r1 = new Record(123, 456);
            var r2 = new Record(456, 789);

            Assert.False(r1 == r2);
            Assert.False(r1.Equals(r2));
            Assert.NotEqual(r1, r2);
            Assert.NotEqual(r1.GetHashCode(), r2.GetHashCode());
            Assert.True(r1 != r2);
        }

        [Fact]
        public void EqualityTests_WhenDiffType_ShouldNotEqual()
        {
            var r1 = new Record(123, 456);
            var r2 = new KeyValuePair<int, int>(456, 789);

            Assert.False(r1.Equals(r2));
            Assert.NotEqual(r1.GetHashCode(), r2.GetHashCode());
        }

        [Fact]
        public void EqualityTests_WhenNull_ShouldNotEqual()
        {
            var r1 = new Record(123, 456);

            Assert.False(r1.Equals(null));
        }
    }
}
