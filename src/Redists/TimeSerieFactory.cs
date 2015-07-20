using Redists.Core;
using Redists.Utils;
using StackExchange.Redis;
using System;
using Redists.Extensions;

namespace Redists
{
    public static class TimeSerieFactory
    {
        public static ITimeSerie New(IDatabaseAsync dbAsync, string name, Settings settings = null)
        {
            Guard.NotNull(dbAsync, "dbAsync");
            Guard.NotNullOrEmpty(name, "name");

            if (!dbAsync.Multiplexer.IsConnected)
                throw new InvalidOperationException("redis connection is not open");

            settings = settings ?? new Settings();
            var parser = settings.UseFixedRecordSize ? (IRecordParser)new FixedRecordParser() : new DynamicRecordParser();
            var reader = new RecordReader(dbAsync, parser);
            var writer = new RecordWriter(dbAsync, parser, settings.SerieTtl);

            return new TimeSerie(name, settings, reader, writer);
        }
    }
}
