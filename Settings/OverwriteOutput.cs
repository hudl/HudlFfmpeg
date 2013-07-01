using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceTypes.Output)]
    public class OverwriteOutput : BaseSetting
    {
        private const string SettingType = "-y";

        public OverwriteOutput()
            : base(SettingType)
        {
        }

        public override string ToString()
        {
            return Type;
        }
    }
}
