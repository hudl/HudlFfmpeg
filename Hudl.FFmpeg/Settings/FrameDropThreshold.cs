using System;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Frame drop threshold, which specifies how much behind video frames can be before they are dropped. In frame rate units, so 1.0 is one frame. The default is -1.1. One possible usecase is to avoid framedrops in case of noisy timestamps or to increase frame drop precision in case of exact timestamps.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "frame_drop_threshold")]
    public class FrameDropThreshold : ISetting
    {
        public FrameDropThreshold()
        {
        }
        public FrameDropThreshold(double threshold)
        {
            Threshold = threshold;
        }

        [SettingParameter]
        public double Threshold { get; set; }
    }
}
