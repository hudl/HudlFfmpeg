using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IAudio))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceTypes.Output)]
    public class AudioBitRate : BaseSetting
    {
        private const string SettingType = "-b:a";

        public AudioBitRate(int rate)
            : base(SettingType)
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
