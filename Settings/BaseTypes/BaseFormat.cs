using System;
using Hudl.Ffmpeg.Common;

namespace Hudl.Ffmpeg.Settings.BaseTypes
{
    public abstract class BaseFormat : BaseSetting
    {
        private const string SettingType = "-f";

        protected BaseFormat(string format)
            : base(SettingType)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                throw new ArgumentNullException("format");
            }

            Format = format;
        }

        protected BaseFormat(FormatType format)
            : this(Formats.Library(format))
        {
        }

        public string Format { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Format))
            {
                throw new InvalidOperationException("Format cannot be empty for this setting.");
            }

            return string.Concat(Type, " ", Format);
        }
    }
}