using Ploeh.AutoFixture;
using Redists.Utils;
using System;
using System.Collections.Generic;
using Xunit;

namespace Redists.Test.Utils
{
    public class GuardTests
    {
        private Fixture fixture = new Fixture();
        private string paramName;

        public GuardTests()
        {
            this.paramName = this.fixture.Create<string>();
        }

        [Fact]
        public void WhenNull_ShouldThrows()
        {
            Assert.Throws<ArgumentNullException>(paramName, () =>
                {
                    Guard.NotNull(null, paramName);
                });
        }

        [Fact]
        public void WhenNotNull_ShouldNotThrows()
        {
            Guard.NotNull(new object(), paramName);
        }

        [Fact]
        public void WhenDefault_ShouldThrows()
        {
            Assert.Throws<ArgumentNullException>(paramName, () =>
            {
                Guard.NotDefault(new KeyValuePair<string, string>(), paramName);
            });
        }

        [Fact]
        public void WhenNotDefault_ShouldNotThrows()
        {
            Guard.NotDefault(new KeyValuePair<string, string>(this.fixture.Create<string>(), this.fixture.Create<string>()), paramName);
        }

        [Fact]
        public void WhenNullOrEmpty_ShouldThrows()
        {
            Assert.Throws<ArgumentNullException>(paramName, () =>
            {
                Guard.NotNullOrEmpty(string.Empty, paramName);
            });
        }

        [Fact]
        public void WhenNotNullOrEmpty_ShouldNotThrows()
        {
            Guard.NotNullOrEmpty(this.fixture.Create<string>(), paramName);
        }
    }
}
