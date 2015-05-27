using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class MapSettingsFormatter : IFormatter
    {
        public string Format(object value)
        {
            return Formats.Map(value.ToString(), true);
        }
    }
}
