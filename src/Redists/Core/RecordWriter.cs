using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redists.Core
{
    internal class RecordWriter
    {
        private readonly IDatabase db;
        private RecordParser parser;

        public RecordWriter(IDatabase db, bool isFixed)
        {
            this.db = db;
            this.parser = new RecordParser(isFixed);
        }

        public Task AppendAsync(string redisKey, Record record)
        {
            var toAppend = parser.Serialize(record) + Constants.InterRecordDelimiter;
            return this.db.StringAppendAsync(redisKey, toAppend); ;
        }
    }
}
