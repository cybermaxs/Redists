using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redists.Core
{
    internal class TimeSeriesReader : ITimeSeriesReader
    {
        private readonly IDatabaseAsync dbAsync;
        private readonly IStringParser<DataPoint> parser;
        public TimeSeriesReader(IDatabaseAsync dbAsync, IStringParser<DataPoint> parser)
        {
            this.dbAsync = dbAsync;
            this.parser = parser;
        }

        public async Task<DataPoint[]> ReadAllAsync(string redisKey)
        {
            // read by batch
            var dataPoints = new List<DataPoint>();
            var partialRaw = string.Empty;
            var cursor = 0;
            while ( (partialRaw = await ReadBlockAsync(redisKey, cursor).ConfigureAwait(false))!=string.Empty)
            {
                var lastindex = partialRaw.LastIndexOf(Constants.InterDelimiterChar);
                var partialRawStrict = partialRaw[partialRaw.Length-1]!=Constants.InterDelimiterChar? partialRaw.Remove(lastindex + 1) : partialRaw;

                dataPoints.AddRange(parser.Parse(partialRawStrict));
                cursor += partialRawStrict.Length;

                if (partialRaw.Length < Constants.BufferSize)
                    break;
            }

            return dataPoints.ToArray();
        }

        private async Task<string> ReadBlockAsync(string redisKey, long start)
        {
            var redisValue = await dbAsync.StringGetRangeAsync(redisKey, start, start + Constants.BufferSize).ConfigureAwait(false);
            return redisValue.ToString();
        }
    }
}
