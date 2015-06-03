using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class SizeHeightFormatter : IFormatter
    {
        public string Format(object value)
        {
            return value.ToString(); 
        }
    }
}
