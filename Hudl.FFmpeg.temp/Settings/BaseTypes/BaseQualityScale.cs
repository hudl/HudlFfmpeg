using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;
using System;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseQualityScale : ISetting
    {
        protected BaseQualityScale(int scale)
        {
            if (scale < 1)
            {
                throw new ArgumentException("Quality scale must be between 1 and 31.");
            }

            if (scale > 31)
            {
                throw new ArgumentException("Quality scale must be between 1 and 31.");
            }

            Scale = scale;
        }

        [SettingParameter]
        [Validate(LogicalOperators.LesserThanOrEqual, 31)]
        public int Scale { get; set; }
    }
}