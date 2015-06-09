using Hudl.FFmpeg.Formatters.Utility;
using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class EnumParameterSlashFormatter : IFormatter
    {
        public string Format(object value)
        {
            return FormattingUtility.EnumValue(value, true);
        }
    }
}
