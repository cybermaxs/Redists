using System;

namespace Redists.Extensions
{
    internal static class Int64Extensions
    {
        public const long EpochTicks = 621355968000000000;

        public static long Normalize(this long value, long factor)
        {
            return value / factor * factor;
        }

        /// <summary>
        /// Convert a timestamp to DateTime.
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long timestamp)
        {
            return new DateTime(TimeSpan.FromSeconds(timestamp).Ticks + EpochTicks, DateTimeKind.Utc);
        }
    }
}
