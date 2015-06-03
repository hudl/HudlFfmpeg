using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;
using Hudl.FFmpeg.Validators;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseProfile : ISetting
    {
        protected BaseProfile(string profile)
        {
            Profile = profile;
        }

        [SettingParameter]
        [Validate(typeof(NullOrWhitespaceValidator))]
        public string Profile { get; set; }
    }

}
