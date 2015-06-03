using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;
using Hudl.FFmpeg.Validators;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseCodec : ISetting
    {
        protected BaseCodec(string codec)
        {
            Codec = codec;
        }

        [SettingParameter]
        [Validate(typeof(NullOrWhitespaceValidator))]
        public string Codec { get; set; }
    }

}
