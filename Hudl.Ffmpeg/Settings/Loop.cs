using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [AppliesToResource(Type = typeof(IAudio))]
    [SettingsApplication(ResourceType = SettingsCollectionResourceType.Input)]
    public class Loop : BaseSetting
    {
        private const string SettingType = "-loop";

        public Loop()
            : base(SettingType)
        {
        }

        public override string ToString()
        {
            return Type + " 1";
        }
    }
}