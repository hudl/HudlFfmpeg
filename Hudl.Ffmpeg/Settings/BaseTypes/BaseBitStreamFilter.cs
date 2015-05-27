using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseBitStreamFilter : ISetting
    {
        protected BaseBitStreamFilter(string setting)
        {
            Setting = setting;
        }

        [SettingValue]
        public string Setting { get; set; }
    }
}
