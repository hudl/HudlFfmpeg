using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;
using Hudl.FFmpeg.Validators;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseBitStreamFilter : ISetting
    {
        protected BaseBitStreamFilter(string setting)
        {
            Setting = setting;
        }

        [SettingParameter]
        [Validate(typeof(NullOrWhitespaceValidator))]
        public string Setting { get; set; }
    }
}
