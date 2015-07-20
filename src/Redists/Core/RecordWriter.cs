using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Redists.Core
{
    internal class RecordWriter : IRecordWriter
    {
        private readonly IDatabaseAsync dbAsync;
        private TimeSpan? ttl;
        private IRecordParser parser;

        public RecordWriter(IDatabaseAsync dbAsync, IRecordParser parser, TimeSpan? ttl)
        {
            this.dbAsync = dbAsync;
            this.parser = parser;
            this.ttl = ttl;
        }

        public Task<long> AppendAsync(string redisKey, Record record)
        {
            this.dbAsync.KeyExpireAsync(redisKey, ttl);
            var toAppend = parser.Serialize(record) + Constants.InterRecordDelimiter;
            return this.dbAsync.StringAppendAsync(redisKey, toAppend);
        }
    }
}
