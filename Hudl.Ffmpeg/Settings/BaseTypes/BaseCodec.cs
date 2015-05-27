using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseCodec : ISetting
    {
        protected BaseCodec(string codec)
        {
            Codec = codec;
        }

        [SettingValue]
        public string Codec { get; set; }
    }

}
