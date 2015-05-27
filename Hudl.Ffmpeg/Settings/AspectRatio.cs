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
        public AspectRatio()
        {
        }
        public AspectRatio(Ratio ratio)
            : this()
        {
            if (ratio == null)
            {
                throw new ArgumentNullException("ratio");
            }

            Ratio = ratio;
        }

        [SettingValue(Formatter = typeof(RatioStringFormatter))]
        public Ratio Ratio { get; set; }
    }
}
