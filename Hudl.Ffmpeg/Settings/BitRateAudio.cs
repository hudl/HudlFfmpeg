using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// sets the audio bit rate for the output stream 
    /// </summary>
    [AppliesToResource(Type = typeof(IAudio))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class BitRateAudio : BaseBitRate
    {
        private const string Suffix = ":a";

        public BitRateAudio(int rate)
            : base(Suffix, rate)
        {
        }
        public BitRateAudio(AudioBitRateType rate)
            : base(Suffix, (int)rate)
        {
        }
    }
}
