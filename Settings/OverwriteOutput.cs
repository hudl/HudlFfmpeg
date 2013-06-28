using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsApplicationAttribute.SettingsResourceType.Output)]
    public class OverwriteOutput : ISetting
    {
        public string Type { get { return "-y"; } }
        
        public override string ToString()
        {
            return Type;
        }
    }
}
