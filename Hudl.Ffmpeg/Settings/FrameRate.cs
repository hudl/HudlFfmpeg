using System;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Set frame rate (Hz value, fraction or abbreviation).
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
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
