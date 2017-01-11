using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// set the logging level used by the target library.
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "loglevel")]
    public class LogLevel : ISetting
    {
        public LogLevel(string level)
        {
            Level = level;
        }

        [SettingParameter]
        public string Level { get; set; }
    }
}
