using System;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class TimeSpanFormatter : IFormatter
    {
        public string Format(object value)
        {
            return Formats.Duration((TimeSpan) value);
        }
    }
}
