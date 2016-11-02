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
    [Setting(Name = "frames:v")]
    public class VideoFrames : ISetting
    {
        public VideoFrames()
        {
        }
        public VideoFrames(int numberOfFrames)
        {
            if (numberOfFrames <= 0)
            {
                throw new ArgumentException("Number of frames must be greater than zero.");
            }

            NumberOfFrames = numberOfFrames;
        }

        [SettingParameter]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public double NumberOfFrames { get; set; }
    }
}
