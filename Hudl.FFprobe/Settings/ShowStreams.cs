using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFprobe.Settings;

[Setting(Name = "show_streams", IsParameterless = true)]
public class ShowStreams : ISetting
{
}