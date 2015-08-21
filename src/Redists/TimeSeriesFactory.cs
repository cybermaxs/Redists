using Redists.Configuration;
using Redists.Core;
using Redists.Utils;
using StackExchange.Redis;
using System;

namespace Redists
{
    public static class TimeSeriesFactory
    {
        public static ITimeSeriesClient New(IDatabaseAsync dbAsync, string name, TimeSeriesOptions settings)
        {
            Guard.NotNull(dbAsync, nameof(dbAsync));
            Guard.NotNullOrEmpty(name, nameof(name));
            Guard.NotNull(settings, nameof(settings));

            if (dbAsync.Multiplexer==null || !dbAsync.Multiplexer.IsConnected)
                throw new InvalidOperationException("redis connection is not open or down");

            var parser = settings.UseFixedSize ? (IStringParser<DataPoint>)new FixedDataPointParser() : new DynamicDataPointParser();
            var reader = new TimeSeriesReader(dbAsync, parser);
            var writer = new TimeSeriesWriter(dbAsync, parser, settings.KeyTtl);

            return new TimeSeriesClient(name, settings, reader, writer);
        }
    }
}
