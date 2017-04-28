using Hudl.FFmpeg.Formatters.Utility;
using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class MapSettingsFormatter : IFormatter
    {
        public string Format(object value)
        {
            return FormattingUtility.Map(value.ToString(), true);
        }
    }
}
