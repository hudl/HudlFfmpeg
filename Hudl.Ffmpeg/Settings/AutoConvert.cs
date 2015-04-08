using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Useful if on the fly scaling is needed for demux concatenation. there are no 
    /// specific settings for this and FFmpeg states that it is good practice to include 
    /// as some files need this setting.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [ForStream(Type = typeof(AudioStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Input)]
    public class AutoConvert : BaseSetting
    {
        private const string SettingType = "-auto_convert";

        public AutoConvert()
            : base(SettingType)
        {
        }

        public override string ToString()
        {
            return string.Concat(Type, " 1");
        }
    }
}