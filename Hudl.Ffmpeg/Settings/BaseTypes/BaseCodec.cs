using System;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseCodec : BaseSetting
    {
        private const string SettingType = "-c";

        protected BaseCodec(string suffix, string codec)
            : base(string.Format("{0}{1}", SettingType, suffix))
        {
            Codec = codec;
        }

        public string Codec { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Codec))
            {
                throw new InvalidOperationException("Codec cannot be empty for this setting.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", Codec);
        }
    }

}
