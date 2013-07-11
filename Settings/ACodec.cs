using System;
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
    public class ACodec : BaseSetting
    {
        private const string SettingType = "-acodec";

        public ACodec(string codec)
            : base(SettingType)
        {
            Codec = codec;
        }
        public ACodec(AudioCodecType codec)
            : this(Formats.Library(codec))
        {
        }

        public string Codec { get; set; }
        
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Codec))
            {
                throw new InvalidOperationException("Codec cannot be empty for this setting.");
            }

            return string.Concat(Type, " ", Codec);
        }
    }
}
