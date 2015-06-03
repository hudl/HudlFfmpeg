using System;
using System.Globalization;
using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class BoolToInt32Formatter : IFormatter
    {
        public string Format(object value)
        {
            return Convert.ToInt32(value).ToString(CultureInfo.InvariantCulture);
        }
    }
}
