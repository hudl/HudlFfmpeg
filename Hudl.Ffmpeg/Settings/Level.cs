using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IAudio))]
    [AppliesToResource(Type = typeof(IVideo))]
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
