using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseProfile : ISetting
    {
        protected BaseProfile(string profile)
        {
            Profile = profile;
        }

        [SettingValue]
        public string Profile { get; set; }
    }

}
