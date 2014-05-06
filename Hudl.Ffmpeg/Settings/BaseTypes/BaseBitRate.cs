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

        public override string ToString()
        {
            if (Rate <= 0)
            {
                throw new InvalidOperationException("Bit Rate must be greater than zero.");
            }

            return string.Concat(Type, " ", Rate, "k");
        }
    }
}
