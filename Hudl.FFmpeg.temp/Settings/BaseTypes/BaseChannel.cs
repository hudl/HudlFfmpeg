using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
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

        [SettingParameter]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public int NumberOfChannels { get; set; }
    }

}
