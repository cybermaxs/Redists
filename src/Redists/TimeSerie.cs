using Redists.Core;
using Redists.Extensions;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redists
{
    internal class TimeSerie : ITimeSerie
    {
        private readonly string name;
        private readonly TimeSerieSettings settings;

        private readonly RecordWriter writer;
        private readonly RecordReader reader;

        public TimeSerie(IDatabase db, string name, TimeSerieSettings settings)
        {
            this.name = name;
            this.settings = settings;

            this.reader = new RecordReader(db, settings.UseFixedRecordSize);
            this.writer = new RecordWriter(db, settings.UseFixedRecordSize);
        }

        public Task AddAsync(long value, DateTime at)
        {
            var tsKey = this.GetRedisKeyName(at);
            var ts = at.ToTimestamp();
            var newRecord = new Record(ts, value);

            return writer.AppendAsync(tsKey, newRecord);
        }

        public async Task<Record[]> AllAsync(DateTime at)
        {
            var dts = at.ToKeyDateTimes(DateTime.UtcNow, this.settings.SerieNormalizeFactor);

            var tasks = new List<Task<Record[]>>();

            foreach(var dt in dts)
            {
                var tsKey = this.GetRedisKeyName(dt);
                tasks.Add(reader.ReadAllAsync(tsKey));
            }
            await Task.WhenAll(tasks.ToArray());
            return tasks.SelectMany(t => t.Result).ToArray();
        }

        public string GetRedisKeyName(DateTime at)
        {
            return "ts#" + this.name + "#" + at.ToRoundedSecondsTimestamp(this.settings.SerieNormalizeFactor);
        }
    }
}
