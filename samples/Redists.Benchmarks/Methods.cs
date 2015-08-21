using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redists.Benchmarks
{
    public sealed class Methods
    {
        private static readonly Random Generator = new Random();

        public static async Task AddAsync(ITimeSeriesClient client)
        {
            var at = DateTime.UtcNow.Date.AddSeconds(Generator.Next(3600 * 24));

            await client.AddAsync(at, Generator.Next());
        }

        public static async Task Add100Async(ITimeSeriesClient client)
        {
            var start = DateTime.UtcNow.Date.AddSeconds(Generator.Next(3600 * 24));

            //Full day
            var tasks = new List<Task>();
            for (var s = 0; s < 100; s++)
            {
                var now = start.AddSeconds(s);
                tasks.Add(client.AddAsync(DateTime.UtcNow, Generator.Next()));
            }

            await Task.WhenAll(tasks.ToArray());
        }

        public static async Task AddBatchof100Async(ITimeSeriesClient client)
        {
            var start = DateTime.UtcNow.Date.AddSeconds(Generator.Next(3600 * 24));

            //Full day
            var points = new List<DataPoint>();
            for (var s = 0; s < 100; s++)
            {
                var now = start.AddSeconds(s);
                points.Add(new DataPoint(now, Generator.Next()));
            }

            await client.AddAsync(points.ToArray());
        }

        public static async Task ReadAllAsync(ITimeSeriesClient client)
        {
            var start = DateTime.UtcNow.Date;
            var end = DateTime.UtcNow.Date.AddHours(1);

            await client.RangeAsync(start, end);
        }
    }
}
