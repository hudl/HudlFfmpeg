using Hudl.FFmpeg.Formatters.Utility;
using Hudl.FFmpeg.Interfaces;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class LocalUriFormatter : IFormatter
    {
        public string Format(object value)
        {
            var escapedPath = FormattingUtility.ConcatenateIfNecessary(((IContainer)value).FullName,((IContainer)value).FullInitName).Replace('\\', '/');
            return string.Concat("\"", escapedPath, "\"");
        }
    }
}
