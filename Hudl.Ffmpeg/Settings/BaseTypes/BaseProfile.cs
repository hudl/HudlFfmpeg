using System;

namespace Hudl.Ffmpeg.Settings.BaseTypes
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
        
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Profile))
            {
                throw new InvalidOperationException("Profile cannot be empty for this setting.");
            }

            return string.Concat(Type, " ", Profile);
        }
    }

}
