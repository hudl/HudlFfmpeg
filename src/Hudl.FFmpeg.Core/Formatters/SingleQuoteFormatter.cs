using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class SingleQuoteFormatter : IFormatter
    {
        public string Format(object value)
        {
            return string.Concat("'", value, "'"); 
        }
    }
}
