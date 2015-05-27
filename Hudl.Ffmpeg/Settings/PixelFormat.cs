using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

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
            : this(Formats.Library(library))
        {
        }

        [SettingValue]
        public string Library { get; set; }
    }
}
