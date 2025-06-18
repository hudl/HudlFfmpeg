using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFprobe.Settings;

[Setting(Name = "show_frames", IsParameterless = true)]
public class ShowFrames : ISetting
{
}