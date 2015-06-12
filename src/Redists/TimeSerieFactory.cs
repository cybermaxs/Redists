using Redists.Utils;
using StackExchange.Redis;

namespace Redists
{
    public static class TimeSerieFactory
    {
        public static ITimeSerie New(IDatabase db, string name, TimeSerieSettings settings=null)
        {
            Guard.NotNull(db, "db");
            Guard.NotNullOrEmpty(name, "name");

            return new TimeSerie(db, name, settings ?? new TimeSerieSettings());
        }
    }
}
