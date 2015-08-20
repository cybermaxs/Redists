using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Redists.Core
{
    internal class TimeSeriesWriter : ITimeSeriesWriter
    {
        private readonly IDatabaseAsync dbAsync;
        private readonly TimeSpan? ttl;
        private readonly IDataPointParser parser;

        private ConcurrentDictionary<string, DateTime> expirations = new ConcurrentDictionary<string, DateTime>();

        public TimeSeriesWriter(IDatabaseAsync dbAsync, IDataPointParser parser, TimeSpan? ttl)
        {
            this.dbAsync = dbAsync;
            this.parser = parser;
            this.ttl = ttl;
        }

        public Task<long> AppendAsync(string redisKey, DataPoint[] dataPoints)
        {
            ManageKeyExpiration(redisKey);
            var toAppend = parser.Serialize(dataPoints) + Constants.InterDelimiter;
            return this.dbAsync.StringAppendAsync(redisKey, toAppend, CommandFlags.FireAndForget);
        }

        private void ManageKeyExpiration(string key)
        {
            if (!this.ttl.HasValue)
                return;

            var lastSent=expirations.GetOrAdd(key, DateTime.MinValue);
            if ((DateTime.UtcNow- lastSent)> this.ttl.Value)
            {
                this.dbAsync.KeyExpireAsync(key, ttl);
                expirations.TryUpdate(key, DateTime.UtcNow, lastSent);
            }
        }
    }
}
