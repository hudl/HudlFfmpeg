using Hudl.FFmpeg.Formatters.Utility;
using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class EnumParameterFormatter : IFormatter
    {
        public string Format(object value)
        {
            return FormattingUtility.EnumDefaultValue(value);
        }
    }
}
