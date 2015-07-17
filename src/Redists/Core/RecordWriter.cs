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
        private RecordParser parser;

        public RecordWriter(IDatabaseAsync dbAsync, bool isFixed, TimeSpan? ttl)
        {
            this.dbAsync = dbAsync;
            this.parser = new RecordParser(isFixed);
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
