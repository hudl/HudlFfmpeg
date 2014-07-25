using System;
using Hudl.FFmpeg.Common;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseFormat : BaseSetting
    {
        private const string SettingType = "-f";

        protected BaseFormat(string format)
            : base(SettingType)
        {
            Format = format;
        }

        protected BaseFormat(FormatType format)
            : this(Formats.Library(format))
        {
        }

        public string Format { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Format))
            {
                throw new InvalidOperationException("Format cannot be empty for this setting.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", Format);
        }
    }
}