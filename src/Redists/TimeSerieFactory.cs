using Redists.Utils;
using StackExchange.Redis;
using System;

namespace Redists
{
    public static class TimeSerieFactory
    {
        public static ITimeSerie New(IDatabase db, string name, TimeSerieSettings settings=null)
        {
            Guard.NotNull(db, "db");
            Guard.NotNullOrEmpty(name, "name");

            if (!db.Multiplexer.IsConnected)
                throw new InvalidOperationException("redis connection is not open");

            return new TimeSerie(db, name, settings ?? new TimeSerieSettings());
        }
    }
}
