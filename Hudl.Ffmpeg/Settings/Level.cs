using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// specifies a video profile target level.
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "level")]
    public class Level : ISetting
    {
        public Level(double setting)
        {
            Setting = setting;
        }
    
        [SettingValue]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public double Setting { get; set; }
    }
}
