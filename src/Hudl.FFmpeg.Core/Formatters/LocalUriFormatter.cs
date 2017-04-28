using Hudl.FFmpeg.Interfaces;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class LocalUriFormatter : IFormatter
    {
        public string Format(object value)
        {
            var escapedPath = ((IContainer)value).FullName.Replace('\\', '/');
            return string.Concat("\"", escapedPath, "\"");
        }
    }
}
