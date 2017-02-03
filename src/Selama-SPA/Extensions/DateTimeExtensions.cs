using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selama_SPA.Extensions
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
