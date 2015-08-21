using System;
using System.Collections.Generic;

namespace Redists.Extensions
{
    internal static class DateTimeExtensions
    {
        public const long EpochTicks = 621355968000000000;
        public const long TicksPeriod = 10000000;
        public const long TicksPeriodMs = 10000;

        //epoch time
        public static readonly DateTime Epoch = new DateTime(EpochTicks, DateTimeKind.Utc);

        /// <summary>
        /// Number of milliseconds since epoch(1/1/1970).
        /// </summary>
        /// <param name="date">DateTime to convert</param>
        /// <returns>Number of milliseconds since 1/1/1970 (Unix timestamp)</returns>
        public static long ToTimestamp(this DateTime date)
        {
            var ts = (date.Ticks - EpochTicks) / TicksPeriodMs;
            return ts;
        }

        /// <summary>
        /// Round a timestamp in ms.
        /// </summary>
        /// <param name="date">DateTime to convert</param>
        /// <param name="factor">Round factor in ms.</param>
        /// <returns>Rounded Timestamp in ms.</returns>
        public static long ToRoundedTimestamp(this DateTime date, long factor)
        {
            return ((long)date.ToTimestamp() / factor) * factor;
        }

        /// <summary>
        /// Number of seconds since epoch(1/1/1970).
        /// </summary>
        /// <param name="date">DateTime to convert</param>
        /// <returns>Number of seconds since 1/1/1970 (Unix timestamp)</returns>
        public static long ToSecondsTimestamp(this DateTime date)
        {
            var ts = (date.Ticks - EpochTicks) / TicksPeriod;
            return ts;
        }

        /// <summary>
        /// Round a timestamp in seconds.
        /// </summary>
        /// <param name="date">DateTime to convert</param>
        /// <param name="factor">Round factor in seconds.</param>
        /// <returns>Rounded Timestamp in seconds.</returns>
        public static long ToRoundedSecondsTimestamp(this DateTime date, long factor)
        {
            return ((long)date.ToSecondsTimestamp() / factor) * factor;
        }


        public static DateTime[] ToKeyDateTimes(this DateTime from, DateTime to, long factor)
        {
            var end = to.ToTimestamp();
            var current = from.ToRoundedTimestamp(factor);
            var nbItems = (end - current) / factor +1;
            var res = new DateTime[nbItems];
            var itemIndex = 0;

            while (current <= end)
            {
                res[itemIndex]=  new DateTime(EpochTicks+ current * TimeSpan.TicksPerMillisecond);
                current += factor;
                itemIndex++;
            }
            return res;
        }
    }
}
