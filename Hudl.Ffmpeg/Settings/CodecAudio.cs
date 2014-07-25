using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Audio Codec for an audio resource file. applies to both audio files, and videos with audio tracks
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class CodecAudio : BaseCodec
    {
        private const string Suffix = ":a";

        public CodecAudio(string codec)
            : base(Suffix, codec)
        {
        }
        public CodecAudio(AudioCodecType codec)
            : base(Suffix, Formats.Library(codec))
        {
        }
    }
}
