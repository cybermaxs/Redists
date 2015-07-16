using StackExchange.Redis;
using System.Threading.Tasks;

namespace Redists.Core
{
    internal class RecordWriter : IRecordWriter
    {
        private readonly IDatabaseAsync dbAsync;
        private RecordParser parser;

        public RecordWriter(IDatabaseAsync dbAsync, bool isFixed)
        {
            this.dbAsync = dbAsync;
            this.parser = new RecordParser(isFixed);
        }

        public Task<long> AppendAsync(string redisKey, Record record)
        {
            var toAppend = parser.Serialize(record) + Constants.InterRecordDelimiter;
            return this.dbAsync.StringAppendAsync(redisKey, toAppend); ;
        }
    }
}
