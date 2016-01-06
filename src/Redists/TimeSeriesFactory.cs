using Redists.Configuration;
using Redists.Core;
using Redists.Utils;
using StackExchange.Redis;
using System;

namespace Redists
{
    /// <summary>
    /// Default Factory for TimeSeries.
    /// Allow to create fixed/dynamic TimeSeries Client.
    /// </summary>
    public static class TimeSeriesFactory
    {
        /// <summary>
        /// Initialize a new TimeSeriesClient with a dynamic datapoint size.
        /// </summary>
        /// <param name="dbAsync">Connected instance of IDatabaseAsync</param>
        /// <param name="name">Name of the TimeSeries (prefix).</param>
        /// <param name="settings">Settings of the TimeSeries.</param>
        /// <returns>An Instance of ITimeSeriesClient</returns>
        public static ITimeSeriesClient New(IDatabaseAsync dbAsync, string name, TimeSeriesOptions settings)
        {
            Guard.NotNull(dbAsync, nameof(dbAsync));
            Guard.NotNullOrEmpty(name, nameof(name));
            Guard.NotNull(settings, nameof(settings));

            if (dbAsync.Multiplexer == null || !dbAsync.Multiplexer.IsConnected)
                throw new InvalidOperationException("redis connection is not opened or closed");

            return CreateNewClient(false, dbAsync, name, settings);
        }

        /// <summary>
        /// Initialize a new TimeSeriesClient with a fixed datapoint size.
        /// </summary>
        /// <param name="dbAsync">Connected instance of IDatabaseAsync</param>
        /// <param name="name">Name of the TimeSeries (prefix).</param>
        /// <param name="settings">Settings of the TimeSeries.</param>
        /// <returns>An Instance of ITimeSeriesClient</returns>
        public static ITimeSeriesClient NewFixed(IDatabaseAsync dbAsync, string name, TimeSeriesOptions settings)
        {
            Guard.NotNull(dbAsync, nameof(dbAsync));
            Guard.NotNullOrEmpty(name, nameof(name));
            Guard.NotNull(settings, nameof(settings));

            if (dbAsync.Multiplexer == null || !dbAsync.Multiplexer.IsConnected)
                throw new InvalidOperationException("redis connection is not opened or closed");

            return CreateNewClient(true, dbAsync, name, settings);
        }

        private static ITimeSeriesClient CreateNewClient(bool isFixed, IDatabaseAsync dbAsync, string name, TimeSeriesOptions settings)
        {
            var parser = isFixed ? (IStringParser <DataPoint>) new FixedDataPointParser() : new DynamicDataPointParser();
            var reader = new TimeSeriesReader(dbAsync, parser);
            var writer = new TimeSeriesWriter(dbAsync, parser, settings.KeyTtl);

            return new TimeSeriesClient(name, settings, reader, writer);
        }
    }
}
