using System;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// When used as an output option (before an output filename), stop writing the output after its duration reaches duration.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [ForStream(Type = typeof(AudioStream))]
    [Setting(Name = "t")]
    public class DurationOutput : BaseDuration
    {
        public DurationOutput(TimeSpan length)
            : base(length)
        {
        }
        public DurationOutput(double seconds)
            : base(seconds)
        {
        }
    }
}
