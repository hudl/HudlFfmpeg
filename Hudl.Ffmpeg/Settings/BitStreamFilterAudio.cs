using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IAudio))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class BitStreamFilterAudio : BaseBitStreamFilter
    {
        private const string Suffix = ":a";

        public const string ConvertAdtsToAsc = "aac_adtstoasc";

        public BitStreamFilterAudio(string setting)
            : base(Suffix, setting)
        {
        }
    }
}
