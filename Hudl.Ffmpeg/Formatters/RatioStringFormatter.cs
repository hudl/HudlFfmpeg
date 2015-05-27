using Hudl.FFmpeg.DataTypes;
using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class RatioStringFormatter : IFormatter
    {
        public string Format(object value)
        {
            return ((Ratio)value).ToString();
        }
    }
}
