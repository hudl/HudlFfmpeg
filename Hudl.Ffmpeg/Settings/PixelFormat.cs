using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Formatters.Utility;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;
using Hudl.FFmpeg.Validators;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// sets the output pix format type.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "pix_fmt")]
    public class PixelFormat : ISetting
    {
        public PixelFormat(string library)
        {
            Library = library;
        }
        public PixelFormat(PixelFormatType library)
            : this(FormattingUtility.Library(library.ToString()))
        {
        }

        [SettingParameter]
        [Validate(typeof(NullOrWhitespaceValidator))]
        public string Library { get; set; }
    }
}
