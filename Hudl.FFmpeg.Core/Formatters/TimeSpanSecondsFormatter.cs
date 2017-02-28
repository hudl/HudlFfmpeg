using System;
using Hudl.FFmpeg.Formatters.Utility;
using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class TimeSpanSecondsFormatter : IFormatter
    {
        public string Format(object value)
        {
            return ((TimeSpan)value).TotalSeconds.ToString();
        }
    }
}
