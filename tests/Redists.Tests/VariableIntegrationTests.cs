using Ploeh.AutoFixture;
using Redists.Configuration;
using Redists.Extensions;
using Redists.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Redists.Tests
{
    [Collection("RedisServer")]
    public class FixedIntegrationTests
    {
        private Fixture fixture;
        private ITimeSeriesClient tsClient;

        public FixedIntegrationTests(RedisServerFixture redisServer)
        {
            this.fixture = new Fixture();
            redisServer.Reset();
            tsClient = TimeSeriesFactory.NewFixed(redisServer.GetDatabase(0), "myts", new TimeSeriesOptions(3600*1000, 1000, TimeSpan.FromHours(1)));
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task UniqueSerie()
        {
            var start = DateTime.UtcNow.Date;

            var tasks = new List<Task>();
            foreach (var i in Enumerable.Range(0, 3600))
            {
                tasks.Add(tsClient.AddAsync( start.AddSeconds(i), i));
            }
            await Task.WhenAll(tasks);

            var r = await tsClient.AllSinceAsync(start);
            Assert.Equal(3600, r.Length);
            foreach (var i in Enumerable.Range(0, 3599))
            {
                Assert.Equal(i, r[i].value);
                Assert.Equal(start.AddSeconds(i).ToRoundedTimestamp(1000), r[i].ts);
            }
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task Multi_Serie()
        {
            var now = DateTime.UtcNow;

            var tasks = new List<Task>();
            foreach (var i in Enumerable.Range(1, 10000))
                tasks.Add(tsClient.AddAsync(now, 1));
            await Task.WhenAll(tasks);

            var r = await tsClient.AllSinceAsync(now);
            Assert.Equal(10000, r.Length);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task Batch()
        {
            var start = DateTime.UtcNow.Date;
            var datas = new List<DataPoint>();
            var tasks = new List<Task>();
            foreach (var i in Enumerable.Range(0, 100))
            {
                datas.Add(new DataPoint(start.AddSeconds(i), i));
            }
            await tsClient.AddAsync(datas.ToArray());

            var r = await tsClient.AllSinceAsync(start);
            Assert.Equal(100, r.Length);
            foreach (var i in Enumerable.Range(0, 99))
            {
                var current = r[i];
                Assert.Equal(i, current.value);
                Assert.Equal(start.AddSeconds(i).ToRoundedTimestamp(1000), current.ts);
            }

        }
    }
}
