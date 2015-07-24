using Redists.Core;
using Redists.Utils;
using StackExchange.Redis;
using System;
using Redists.Extensions;
using Redists.Configuration;

namespace Redists
{
    public static class TimeSeriesFactory
    {
        public static ITimeSeriesClient New(IDatabaseAsync dbAsync, string name, TimeSeriesOptions settings = null)
        {
            Guard.NotNull(dbAsync, "dbAsync");
            Guard.NotNullOrEmpty(name, "name");

            if (dbAsync.Multiplexer==null || !dbAsync.Multiplexer.IsConnected)
                throw new InvalidOperationException("redis connection is not open");

            settings = settings ?? new TimeSeriesOptions();

            var parser = settings.UseFixedRecordSize ? (IRecordParser)new FixedRecordParser() : new DynamicRecordParser();
            var reader = new RecordReader(dbAsync, parser);
            var writer = new RecordWriter(dbAsync, parser, settings.Ttl);

            return new TimeSeriesClient(name, settings, reader, writer);
        }
    }
}
