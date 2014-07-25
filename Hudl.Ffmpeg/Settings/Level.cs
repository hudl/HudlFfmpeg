using System;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// specifies a video profile target level.
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class Level : BaseSetting
    {
        private const string SettingType = "-level";

        public Level(double setting)
            : base(SettingType)
        {
            Setting = setting;
        }
    
        public double Setting { get; set; }

        public override void Validate()
        {
            if (Setting <= 0)
            {
                throw new InvalidOperationException("Setting size must be greater than zero.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", Setting);
        }
    }
}
