using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceTypes.Output)]
    public class FrameRate : BaseSetting
    {
        private const string SettingType = "-r";

        public FrameRate(double rate)
            : base(SettingType)
        {
            if (rate <= 0)
            {
                throw new ArgumentException("Frame rate must be greater than zero.");
            }

            Rate = rate;
        }

        public double Rate { get; set; }

        public override string ToString()
        {
            if (Rate <= 0)
            {
                throw new InvalidOperationException("Frame rate must be greater than zero.");
            }

            return string.Concat(Type, " ", Rate);
        }
    }
}
