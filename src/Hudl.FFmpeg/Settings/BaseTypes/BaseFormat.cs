using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Formatters.Utility;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;
using Hudl.FFmpeg.Validators;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseFormat : ISetting
    {
        protected BaseFormat(string format)
        {
            Format = format;
        }
        protected BaseFormat(FormatType format)
            : this(FormattingUtility.Library(format.ToString()))
        {
        }

        [SettingParameter]
        [Validate(typeof(NullOrWhitespaceValidator))]
        public string Format { get; set; }
    }
}