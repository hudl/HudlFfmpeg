using System;

namespace Hudl.Ffmpeg.Settings.BaseTypes
{
    public abstract class BaseBitRate : BaseSetting
    {
        private const string SettingType = "-b";

        protected BaseBitRate(string suffix, int rate)
            : base(string.Format("{0}{1}", SettingType, suffix))
        {
            Rate = rate;
        }

        public int Rate { get; set; }

        public override void Validate()
        {
            if (Rate <= 0)
            {
                throw new InvalidOperationException("Bit Rate must be greater than zero.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", Rate, "k");
        }
    }
}
