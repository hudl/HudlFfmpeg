using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// Set frame rate (Hz value, fraction or abbreviation).
    /// </summary>
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class FrameRate : BaseSetting
    {
        private const string SettingType = "-r";

        public FrameRate()
            : base(SettingType)
        {
        }
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

        public override void Validate()
        {
            if (Rate <= 0)
            {
                throw new InvalidOperationException("Frame rate must be greater than zero.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", Rate);
        }
    }
}
