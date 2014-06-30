using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class BitStreamFilterVideo : BaseBitStreamFilter
    {
        private const string Suffix = ":v";

        public BitStreamFilterVideo(string setting)
            : base(Suffix, setting)
        {
        }
    }
}
