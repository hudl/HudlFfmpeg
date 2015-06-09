using Hudl.FFmpeg.Interfaces;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class LocalUriFormatterNoWrap : IFormatter
    {
        public string Format(object value)
        {
            return ((IContainer)value).FullName.Replace('\\', '/');
        }
    }
}
