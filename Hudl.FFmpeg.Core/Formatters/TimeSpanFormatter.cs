using System;
using Hudl.FFmpeg.Formatters.Utility;
using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class TimeSpanFormatter : IFormatter
    {
        public string Format(object value)
        {
            return FormattingUtility.Duration((TimeSpan) value);
        }
    }
}
