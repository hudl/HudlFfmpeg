using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Formatters.Utility;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;
using Hudl.FFmpeg.Validators;
using System;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseFrameRate : ISetting
    {
        public BaseFrameRate()
        {
        }
        public BaseFrameRate(double rate)
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