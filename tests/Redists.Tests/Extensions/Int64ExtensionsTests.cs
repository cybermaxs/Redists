using Xunit;
using Redists.Extensions;

namespace Redists.Tests.Extensions
{
    public class Int64ExtensionsTests
    {
        [Theory]
        [InlineData(160L,100L, 100L)]
        [InlineData(293123L, 10L, 293120L)]
        [InlineData(12319230L, 600L, 12319200L)]
        public void Normalization(long input, long factor,  long expected)
        {
            Assert.Equal(expected, input.Normalize(factor));
        }
    }
}
