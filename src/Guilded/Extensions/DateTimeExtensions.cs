using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guilded.Extensions
{
    public static class DateTimeExtensions
    {
        private const int SecInMin = 60;
        private const int SecInHr = SecInMin * 60;
        private const int SecInDay = SecInHr * 24;
        private const int SecInWeek = SecInDay * 7;
        private const int SecInMonth = SecInDay * 30;
        private const int SecInYear = SecInDay * 365;

        private static DateTime? _now;

        public static DateTime Now
        {
            get => _now ?? DateTime.Now;
            set => _now = value;
        }

        public static string ToRelativeTimeStamp(this DateTime dateToFormat)
        {
            var now = Now;

            if (dateToFormat.Kind == DateTimeKind.Utc)
            {
                now = now.ToUniversalTime();
            }

            var timeDiff = new TimeSpan(now.Ticks - dateToFormat.Ticks);
            var isFutureTime = timeDiff.TotalSeconds < 0;
            var timeWithLabel = TimeWithLabel(Math.Abs(timeDiff.TotalSeconds));

            if (isFutureTime)
            {
                return $"In {timeWithLabel}";
            }

            return $"{timeWithLabel} ago";
        }

        private static string TimeWithLabel(double timeDiffInSeconds)
        {
            if (timeDiffInSeconds < SecInMin)
            {
                return FormatTimeWithLabel((int)timeDiffInSeconds, "second");
            }
            if (timeDiffInSeconds < SecInHr)
            {
                return FormatTimeWithLabel((int)(timeDiffInSeconds / SecInMin), "minute");
            }
            if (timeDiffInSeconds < SecInDay)
            {
                return FormatTimeWithLabel((int)(timeDiffInSeconds / SecInHr), "hour");
            }
            if (timeDiffInSeconds < SecInWeek)
            {
                return FormatTimeWithLabel((int)(timeDiffInSeconds / SecInDay), "day");
            }
            if (timeDiffInSeconds < SecInMonth)
            {
                return FormatTimeWithLabel((int)(timeDiffInSeconds / SecInWeek), "week");
            }
            if (timeDiffInSeconds < SecInYear)
            {
                return FormatTimeWithLabel((int)(timeDiffInSeconds / SecInMonth), "month");
            }

            return FormatTimeWithLabel((int)(timeDiffInSeconds / SecInYear), "year");
        }

        private static string FormatTimeWithLabel(int timeDiff, string label)
        {
            if (timeDiff != 1)
            {
                label += "s";
            }
            return $"{timeDiff} {label}";
        }
    }
}
