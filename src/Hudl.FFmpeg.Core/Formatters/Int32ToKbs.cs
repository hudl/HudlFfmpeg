using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class Int32ToKbs : IFormatter
    {
        public string Format(object value)
        {
            return string.Concat(value, "k"); 
        }
    }
}
