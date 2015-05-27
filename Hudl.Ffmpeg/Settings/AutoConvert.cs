using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Useful if on the fly scaling is needed for demux concatenation. there are no 
    /// specific settings for this and FFmpeg states that it is good practice to include 
    /// as some files need this setting.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [ForStream(Type = typeof(AudioStream))]
    [Setting(Name = "auto_convert", ResourceType = SettingsCollectionResourceType.Input)]
    public class AutoConvert : ISetting
    {
        public AutoConvert()
        {
        }

        [SettingValue(Formatter = typeof(BoolToInt32Formatter))]
        public bool IsOn { get; set; }
    }
}