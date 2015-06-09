using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Formatters.Utility;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Video codec for a video resource file.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "c:v")]
    public class CodecVideo : BaseCodec
    {
        public CodecVideo(string codec)
            : base(codec)
        {
        }
        public CodecVideo(VideoCodecType codec)
            : base(FormattingUtility.Library(codec.ToString()))
        {
        }
    }
}
