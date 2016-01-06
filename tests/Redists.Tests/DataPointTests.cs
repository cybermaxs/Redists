using System;
using System.Collections.Generic;
using Xunit;

namespace Redists.Tests
{
    public class DataPointTests
    {
        [Fact]
        public void Ctor_ByDate_ShouldBeSame()
        {
            var dp1 = new DataPoint(1437724399000, 456);
            var dp2 = new DataPoint(new DateTime(2015, 7, 24, 7, 53, 19, DateTimeKind.Utc), 456);

            Assert.Equal(dp1.timestamp, dp2.timestamp);
        }

        [Fact]
        public void ToString_ShouldReturnCorrectValue()
        {
            var dp = new DataPoint(123, 456);
            var str = dp.ToString();
            Assert.Equal("123:456", str);
        }

        [Fact]
        public void EqualityTests_WhenSame_ShouldBeEqual()
        {
            var dp1 = new DataPoint(123, 456);
            var dp2 = new DataPoint(123, 456);

            Assert.True(dp1 == dp2);
            Assert.True(dp1.Equals(dp2));
            Assert.Equal(dp1.GetHashCode(), dp2.GetHashCode());
            Assert.Equal(dp1, dp2);
            Assert.False(dp1 != dp2);
        }

        [Fact]
        public void EqualityTests_WhenDiff__ShouldNotEqual()
        {
            var dp1 = new DataPoint(123, 456);
            var dp2 = new DataPoint(456, 789);

            Assert.False(dp1 == dp2);
            Assert.False(dp1.Equals(dp2));
            Assert.NotEqual(dp1, dp2);
            Assert.NotEqual(dp1.GetHashCode(), dp2.GetHashCode());
            Assert.True(dp1 != dp2);
        }

        [Fact]
        public void EqualityTests_WhenDiffType_ShouldNotEqual()
        {
            var dp1 = new DataPoint(123, 456);
            var dp2 = new KeyValuePair<int, int>(456, 789);

            Assert.False(dp1.Equals(dp2));
            Assert.NotEqual(dp1.GetHashCode(), dp2.GetHashCode());
        }

        [Fact]
        public void EqualityTests_WhenNull_ShouldNotEqual()
        {
            var dp1 = new DataPoint(123, 456);

            Assert.False(dp1.Equals(null));
        }

        [Fact]
        public void Normalize_WhenValue_ShouldBeRounded()
        {
            var dp = new DataPoint(123, 456);

            dp.Normalize(10);

            Assert.Equal(120, dp.timestamp);
        }
    }
}
