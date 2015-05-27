using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseFormat : ISetting
    {
        protected BaseFormat(string format)
        {
            Format = format;
        }
        protected BaseFormat(FormatType format)
            : this(Formats.Library(format))
        {
        }

        [SettingValue]
        public string Format { get; set; }
    }
}