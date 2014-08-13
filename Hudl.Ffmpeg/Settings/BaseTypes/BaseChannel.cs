using System;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseChannel : BaseSetting
    {
        private const string SettingType = "-ac";
        
        protected BaseChannel(int numberOfChannels)
            : base(SettingType)
        {
            NumberOfChannels = numberOfChannels;
        }

        public int NumberOfChannels { get; set; }

        public override void Validate()
        {
            if (NumberOfChannels <= 0)
            {
                throw new InvalidOperationException("NumberOfChannels must be greater than zero.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", NumberOfChannels);
        }
    }

}
