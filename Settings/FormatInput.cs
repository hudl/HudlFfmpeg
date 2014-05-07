using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [AppliesToResource(Type = typeof(IAudio))]
    [AppliesToResource(Type = typeof(IImage))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Input)]
    public class FormatInput : BaseFormat
    {
        public FormatInput(string format)
            : base(format)
        {
        }

        public FormatInput(FormatType format)
            : base(format)
        {
        }
    }
}