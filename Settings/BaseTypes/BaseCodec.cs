using System;

namespace Hudl.Ffmpeg.Settings.BaseTypes
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
