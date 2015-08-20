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
            this.prefix = $"ts#" + name + "#";
            this.settings = settings;
            this.reader = reader;
            this.writer = writer;
        }

        public Task AddAsync(DateTime at, long value)
        {
            var dataPoint = new DataPoint(at, value);
            dataPoint.Normalize(this.settings.DataPointNormFactor);
            var key = this.GetRedisKeyName(dataPoint.ts);
            return writer.AppendAsync(key, new DataPoint[] { dataPoint });
        }

        public Task AddAsync(params DataPoint[] dataPoints)
        {
            if (dataPoints == null || dataPoints.Length == 0)
                return Task.FromResult<object>(null);

            var tasks = new List<Task>();

            //normalize all dataPoints first
            foreach (var dataPoint in dataPoints)
            {
                dataPoint.Normalize(this.settings.DataPointNormFactor);
            }

            //group by time series
            foreach (var serie in dataPoints.GroupBy(k => this.GetRedisKeyName(k.ts)))
            {
                var tsKey = serie.Key;
                tasks.Add(writer.AppendAsync(tsKey, serie.ToArray()));
            }
            return Task.WhenAll(tasks.ToArray());
        }

        public async Task<DataPoint[]> AllAsync(DateTime from, DateTime? to = null)
        {
            var dts = from.ToKeyDateTimes(to ?? DateTime.UtcNow, this.settings.KeyNormFactor);

            var tasks = new List<Task<DataPoint[]>>();

            foreach (var dt in dts)
            {
                var tsKey = this.GetRedisKeyName(dt.ToTimestamp());
                tasks.Add(reader.ReadAllAsync(tsKey));
            }
            await Task.WhenAll(tasks.ToArray());
            return tasks.SelectMany(t => t.Result).ToArray();
        }

        private string GetRedisKeyName(long ts)
        {
            return this.prefix + ts.Normalize(this.settings.KeyNormFactor).ToString();
        }
    }
}
