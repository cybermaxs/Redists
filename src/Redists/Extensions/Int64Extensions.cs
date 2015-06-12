namespace Redists.Extensions
{
    internal static class Int64Extensions
    {
        public static long Normalize(this long value, long factor)
        {
            return (long)(value / factor) * factor;
        }
    }
}
