using Redists.Configuration;
using Redists.Core;
using Redists.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redists
{
    internal class TimeSeriesClient : ITimeSeriesClient
    {
        private readonly string prefix;
        private readonly TimeSeriesOptions settings;
        private readonly ITimeSeriesWriter writer;
        private readonly ITimeSeriesReader reader;

        public TimeSeriesClient(string name, TimeSeriesOptions settings, ITimeSeriesReader reader, ITimeSeriesWriter writer)
        {
            prefix = $"{settings.Prefix}{settings.Delimiter}{name}{settings.Delimiter}";
            this.settings = settings;
            this.reader = reader;
            this.writer = writer;
        }

        public Task<long> AddAsync(DateTime at, long value)
        {
            var dataPoint = new DataPoint(at, value);
            dataPoint.Normalize(settings.DataPointNormFactor);
            var key = GetRedisKeyName(dataPoint.timestamp);
            return writer.AppendAsync(key, new DataPoint[] { dataPoint });
        }

        public async Task<long> AddAsync(params DataPoint[] dataPoints)
        {
            if (dataPoints == null || dataPoints.Length == 0)
                return 0L;

            //group by redis key
            var groups = new Dictionary<string, List<DataPoint>>();
            foreach (var dataPoint in dataPoints)
            {
                dataPoint.Normalize(this.settings.DataPointNormFactor);
                var redisKey = this.GetRedisKeyName(dataPoint.timestamp);
                List<DataPoint> list;
                if(!groups.TryGetValue(redisKey, out list))
                {
                    list = new List<DataPoint>();
                    groups.Add(redisKey, list);
                }
                list.Add(dataPoint);
            }

            //batchs
            var tasks = new List<Task<long>>();
            foreach (var serie in groups)
            {
                var tsKey = serie.Key;
                tasks.Add(writer.AppendAsync(tsKey, serie.Value.ToArray()));
            }
            await Task.WhenAll(tasks.ToArray()).ConfigureAwait(false);

            return tasks.Sum(t => t.Result);
        }

        public Task<DataPoint[]> AllSinceAsync(DateTime from)
        {
            if (from.Kind != DateTimeKind.Utc)
                throw new ArgumentException("from should be an UTC datetime.");

            return RangeAsync(from, DateTime.UtcNow);
        }
        public async Task<DataPoint[]> RangeAsync(DateTime from, DateTime to)
        {
            if (from.Kind != DateTimeKind.Utc || to.Kind != DateTimeKind.Utc)
                throw new ArgumentException("from/to should be UTC datetimes.");

            var dts = from.ToKeyDateTimes(to , this.settings.KeyNormFactor);

            var tasks = new List<Task<DataPoint[]>>();

            foreach (var dt in dts)
            {
                var tsKey = GetRedisKeyName(dt.ToTimestamp());
                tasks.Add(reader.ReadAllAsync(tsKey));
            }
            await Task.WhenAll(tasks.ToArray()).ConfigureAwait(false);
            return tasks.SelectMany(t => t.Result).ToArray();
        }

        private string GetRedisKeyName(long ts)
        {
            return prefix + ts.Normalize(settings.KeyNormFactor).ToString();
        }
    }
}
