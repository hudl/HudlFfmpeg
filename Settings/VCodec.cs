using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsApplicationAttribute.SettingsResourceType.Output)]
    public class VCodec : ISetting
    {
        public VCodec(string codec)
        {
            Codec = codec;
        }
        public VCodec(VideoCodecTypes codec)
            : this(Formats.FormatCodec(codec))
        {
        }

        public string Codec { get; set; }

        public string Type { get { return "-vcodec"; } }
        
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
