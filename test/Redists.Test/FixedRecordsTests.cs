using Ploeh.AutoFixture;
using Redists.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Redists.Test
{
    public class FixedRecordsTests : IClassFixture<RedisServerFixture>
    {
        private Fixture fixture;
        private ITimeSerie tsClient;

        public FixedRecordsTests(RedisServerFixture redisServer)
        {
            this.fixture = new Fixture();
            redisServer.Reset();
            tsClient = TimeSerieFactory.New(redisServer.GetDatabase(0), "myts", new TimeSerieSettings(3600, 60, true));
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void BasicGetSet()
        {

            tsClient.AddAsync(1, DateTime.UtcNow).Wait();
            tsClient.AddAsync(2, DateTime.UtcNow).Wait();
            tsClient.AddAsync(3, DateTime.UtcNow).Wait();
            tsClient.AddAsync(4, DateTime.UtcNow).Wait();

            Assert.Equal(4, tsClient.AllAsync(DateTime.UtcNow).Result.Length);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task Multi()
        {
            //var tasks = new List<Task>();
            foreach (var i in Enumerable.Range(1, 1000))
                await tsClient.AddAsync(1, DateTime.UtcNow);
            //await Task.WhenAll(tasks);

            foreach (var i in Enumerable.Range(1, 100))
            {
                var r = await tsClient.AllAsync(DateTime.UtcNow);
                Assert.Equal(1000, r.Length);
            }
        }
    }
}
