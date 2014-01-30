using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// Video codec for a video resource file.
    /// </summary>
    [AppliesToResource(Type = typeof(IVideo))]
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
