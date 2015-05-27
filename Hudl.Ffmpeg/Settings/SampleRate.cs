using System;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.BaseTypes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    [ForStream(Type = typeof(AudioStream))]
    [Setting(Name = "ar")]
    public class SampleRate : ISetting
    {
        public SampleRate(double rate)
        {
            if (rate <= 0)
            {
                throw new ArgumentException("Sample rate must be greater than zero.");
            }

            Rate = rate;
        }

        [SettingValue]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public double Rate { get; set; }
    }
}
