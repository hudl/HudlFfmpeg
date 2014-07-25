using System;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseProfile : BaseSetting
    {
        private const string SettingType = "-profile";

        protected BaseProfile(string suffix, string profile)
            : base(string.Format("{0}{1}", SettingType, suffix))
        {
            Profile = profile;
        }

        public string Profile { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Profile))
            {
                throw new InvalidOperationException("Profile cannot be empty for this setting.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", Profile);
        }
    }

}
