using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hudl.FFmpeg.Extensions
{
    public static class TimespanExtensions
    {
        public static TimeSpan Milliseconds(this int value)
        {
            return TimeSpan.FromMilliseconds(value);
        }

        public static TimeSpan Milliseconds(this double value)
        {
            return TimeSpan.FromMilliseconds(value);
        }

        public static TimeSpan Seconds(this int value)
        {
            return TimeSpan.FromSeconds(value);
        }

        public static TimeSpan Seconds(this double value)
        {
            return TimeSpan.FromSeconds(value);
        }

        public static TimeSpan Minutes(this int value)
        {
            return TimeSpan.FromMinutes(value);
        }

        public static TimeSpan Minutes(this double value)
        {
            return TimeSpan.FromMinutes(value);
        }
    }
}
