using System.Drawing;
using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class SizeFormatter : IFormatter
    {
        public string Format(object value)
        {
            var sizeValue = (Size) value;
            return string.Format("{0}x{1}", sizeValue.Width, sizeValue.Height);
        }
    }
}
