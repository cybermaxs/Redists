using Moq;
using Ploeh.AutoFixture;
using Redists.Core;
using StackExchange.Redis;
using Xunit;

namespace Redists.Test.Core
{
    public class VariableRecordWriterTests
    {
        private readonly RecordWriter writer;
        private Fixture fixture = new Fixture();
        private Mock<IDatabase> mockOfDb;

        public VariableRecordWriterTests()
        {
            mockOfDb = new Mock<IDatabase>();
            mockOfDb.Setup(db => db.StringAppendAsync(It.IsAny<string>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>())).ReturnsAsync(10);

            writer = new RecordWriter(mockOfDb.Object, false);
        }

        [Fact]
        public void WriteShouldPass()
        {
            var key = this.fixture.Create<string>();
            var record = new Record(123, 456);
            var t = writer.AppendAsync(key, record);

            Assert.NotNull(t);
            t.Wait();
            Assert.False(t.IsFaulted);
            Assert.False(t.IsCanceled);

            mockOfDb.Verify(db => db.StringAppendAsync(key, "123:456¤", It.IsAny<CommandFlags>()), Times.Once);
        }
    }
}
