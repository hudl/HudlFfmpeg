using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsApplicationAttribute.SettingsResourceType.Output)]
    public class ACodec : ISetting
    {
        public ACodec(string codec)
        {
            Codec = codec;
        }
        public ACodec(AudioCodecTypes codec)
            : this(Formats.Library(codec))
        {
        }

        public string Codec { get; set; }

        public string Type { get { return "-acodec"; } }
        
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Codec))
            {
                throw new ArgumentException("Codec cannot be empty for this setting.");
            }

            return string.Concat(Type, " ", Codec);
        }
    }
}
