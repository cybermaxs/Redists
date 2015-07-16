using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redists.Extensions;

namespace Redists.Core
{
    internal class RecordReader : IRecordReader
    {
        private readonly IDatabaseAsync dbAsync;
        private readonly bool fixedRecordSize;
        private readonly RecordParser parser;
        public RecordReader(IDatabaseAsync dbAsync, bool fixedRecordSize)
        {
            this.dbAsync = dbAsync;
            this.fixedRecordSize = fixedRecordSize;
            this.parser = new RecordParser(fixedRecordSize);
        }

        public async Task<Record[]> ReadAllAsync(string redisKey)
        {
            ///read by batch
            List<Record> records = new List<Record>();
            string partialRaw = string.Empty;
            int cursor = 0;
            while ( (partialRaw = await ReadBlockAsync(redisKey, cursor).ConfigureAwait(false))!=string.Empty)
            {
                var lastindex = partialRaw.LastIndexOf(Constants.InterRecordDelimiter);
                var partialRawStrict = partialRaw[partialRaw.Length-1]!=Constants.InterRecordDelimiter ? partialRaw.Remove(lastindex + 1) : partialRaw;

                records.AddRange(parser.ParseRawString(partialRawStrict));
                cursor += partialRawStrict.Length;

                if (partialRaw.Length < Constants.BufferSize)
                    break;
            }

            return records.ToArray();
        }

        private async Task<string> ReadBlockAsync(string redisKey, int start)
        {
            var raw = (string)await this.dbAsync.StringGetRangeAsync(redisKey, start, start + Constants.BufferSize).ConfigureAwait(false);
            return raw;
        }
    }
}
