using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Video codec for a video resource file.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class CodecVideo : BaseCodec
    {
        private const string Suffix = ":v";

        public CodecVideo(string codec)
            : base(Suffix, codec)
        {
        }
        public CodecVideo(VideoCodecType codec)
            : base(Suffix, Formats.Library(codec))
        {
        }
    }
}
