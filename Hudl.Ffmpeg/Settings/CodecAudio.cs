using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// Audio Codec for an audio resource file. applies to both audio files, and videos with audio tracks
    /// </summary>
    [AppliesToResource(Type = typeof(IAudio))]
    [AppliesToResource(Type = typeof(IVideo))]
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
