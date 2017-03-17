using System;

namespace Guilded.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnixEpochTime(this DateTime value)
        {
            return (long)Math.Round((value.ToUniversalTime() -
                new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }
    }
}
