using System;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Start At can only be used on the first input resource stream. FFmpeg will not process the video until the starting point provided.
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "ss")]
    public class SeekPositionOutput : BaseSeekPosition
    {
        public SeekPositionOutput(TimeSpan length)
            : base(length)
        {
        }
        public SeekPositionOutput(double seconds)
            : this(TimeSpan.FromSeconds(seconds))
        {
        }
    }
}
