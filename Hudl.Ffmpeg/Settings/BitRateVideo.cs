using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// sets the video bitrate for the output stream
    /// </summary>
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class BitRateVideo : BaseBitRate
    {
        private const string Suffix = ":v";

        public BitRateVideo(int rate)
            : base(Suffix, rate)
        {
        }
    }
}
