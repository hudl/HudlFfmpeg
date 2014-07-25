using System;

namespace Hudl.Ffmpeg.Settings.BaseTypes
{
    public abstract class BaseBitStreamFilter : BaseSetting
    {
        private const string SettingType = "-bsf";

        protected BaseBitStreamFilter(string suffix, string setting)
            : base(string.Format("{0}{1}", SettingType, suffix))
        {
            Setting = setting;
        }

        public string Setting { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Setting))
            {
                throw new InvalidOperationException("Bitstream filter must not be null or whitespace.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", Setting);
        }
    }
}
