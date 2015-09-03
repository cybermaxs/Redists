using Ploeh.AutoFixture;
using Redists.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Redists.Extensions;
using Redists.Configuration;

namespace Redists.Tests
{
    [Collection("RedisServer")]
    public class VariableIntegrationTests
    {
        private Fixture fixture;
        private ITimeSeriesClient tsClient;

        public VariableIntegrationTests(RedisServerFixture redisServer)
        {
            this.fixture = new Fixture();
            redisServer.Reset();
            tsClient = TimeSeriesFactory.New(redisServer.GetDatabase(0), "myts", new TimeSeriesOptions(3600*1000, 1000, TimeSpan.FromHours(1)));
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task Sequence()
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
        public async Task Multi()
        {
            var now = DateTime.UtcNow;

            var tasks = new List<Task>();
            foreach (var i in Enumerable.Range(1, 1000))
                tasks.Add(tsClient.AddAsync( now, 1));
            await Task.WhenAll(tasks);

            var r = await tsClient.AllSinceAsync(now);
            Assert.Equal(1000, r.Length);
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
                Assert.Equal(i, r[i].value);
                Assert.Equal(start.AddSeconds(i).ToRoundedTimestamp(1000), r[i].ts);
            }

        }
    }
}
