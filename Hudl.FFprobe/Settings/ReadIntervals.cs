using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFprobe.Settings;

[Setting(Name = "read_intervals")]
public class ReadIntervals : ISetting
{
    public ReadIntervals(string expression)
    {
        Expression = expression;
    }

    [SettingParameter]
    public string Expression { get; set; }
}