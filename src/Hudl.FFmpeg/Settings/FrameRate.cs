using System;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Set frame rate (Hz value, fraction or abbreviation).
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "r")]
    public class FrameRate : ISetting
    {
        public FrameRate()
        {
        }
        public FrameRate(double rate)
        {
            if (rate <= 0)
            {
                throw new ArgumentException("Frame rate must be greater than zero.");
            }

            Rate = rate;
        }

        [SettingParameter]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public double Rate { get; set; }
    }
}
