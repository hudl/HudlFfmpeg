using System;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.DataTypes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// set the video display aspect ratio specified by aspect.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "aspect")]
    public class AspectRatio : ISetting
    {
        public AspectRatio(Ratio ratio)
        {
            if (ratio == null)
            {
                throw new ArgumentNullException("ratio");
            }

            Ratio = ratio;
        }

        [SettingParameter(Formatter = typeof(RatioStringFormatter))]
        [Validate(LogicalOperators.NotEquals, null)]
        public Ratio Ratio { get; set; }
    }
}
