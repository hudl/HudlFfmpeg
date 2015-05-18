using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Filters.Interfaces;

namespace Hudl.FFmpeg.Filters.Formatters
{
    public class EnumParameterFormatter : IFilterParameterFormatter
    {
        public string Format(object value)
        {
            return Formats.EnumValue(value);
        }
    }
}
