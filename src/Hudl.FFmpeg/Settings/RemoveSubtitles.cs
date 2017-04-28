using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// removes the subtitles stream from the output file
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "sn", IsParameterless = true)]
    public class RemoveSubtitles : ISetting
    {
    }
}
