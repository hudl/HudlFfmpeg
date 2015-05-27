using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseChannel : ISetting
    {
        protected BaseChannel(int numberOfChannels)
        {
            NumberOfChannels = numberOfChannels;
        }

        [SettingValue]
        public int NumberOfChannels { get; set; }
    }

}
