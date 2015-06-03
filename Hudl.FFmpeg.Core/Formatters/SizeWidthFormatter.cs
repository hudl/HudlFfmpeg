using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class SizeWidthFormatter : IFormatter
    {
        public string Format(object value)
        {
            return string.Format("{0}x", value);
        }
    }
}
