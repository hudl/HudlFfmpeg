using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Formatters.Utility;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Audio Codec for an audio resource file. applies to both audio files, and videos with audio tracks
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "c:a")]
    public class CodecAudio : BaseCodec
    {
        public CodecAudio(string codec)
            : base(codec)
        {
        }
        public CodecAudio(AudioCodecType codec)
            : base(FormattingUtility.Library(codec.ToString()))
        {
        }
    }
}
